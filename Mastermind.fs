: rl s" Mastermind.fs" included ; 
\ Usage:
\ >n cg
\ generates the n-th possible solution in the given system (depending on colors and fields)
\ >[guess] sol! 
\ defines the correct solution
\ >[guess] chk
\ checks the guess - outputs the number of correct positions and colors 
\ >[guess] [guess] check 
\ outputs the number of matching positions and colors 
\ have fun!
\ Playing:
\ >init
\ >[guess] ??
\ >shittyknuth
\ to solve the riddle with knuths algorithm
\ [guess] always refers to "fields" numbers ( in interval [0,colors[ ) 

6 constant colors
4 constant fields


: exp ( u1 u2 -- u3 ) \ u3 = u1^u2
   over swap 1 ?do over * loop nip ;
   
: nsol ( -- n ) [ colors fields exp ] literal ;
: 4x4roll 4 roll 4 roll 4 roll 4 roll ;
: cg ( number  -- u*fields)
	nsol mod
	fields colors rot
	0 3 pick 1 -
	-DO
		over i exp
		2dup /
		4x4roll
		mod
	1 -LOOP
	rot rot drop drop ;


: g ( -- [guess] ) \ for testing purposes generates guess [ 0 .. fields]
	fields  0  +DO 
		i
	1 +LOOP ;

: guessover ( [guess1] [guess2] -- [guess1] [guess2] [guess1] )
	fields 0 +DO
		fields 2 * -1 + pick
	1 +LOOP ;

: count ( [guess] n1 -- n1 u )
\ how often is color n in guess u*fields
\ leaves to color and the number on the stack
	0
	fields 0 +DO
		over 3 roll = + 
	1 +LOOP
	negate
; 



: countc ( [guess1] [guess2] n -- u )  \ count number of correct occurences of number n in guess1
	count >r
	count r> 
	min
	swap drop
;

: checkc ( [guess] [guess]  -- u ) \ check number of correct colors
	colors 1 +DO
		guessover guessover
	1 +LOOP
	0
	colors 0 +DO
		i swap >r countc r> +  \ ugly hack to make return stack work in loop
	1 +LOOP
;

: check ( [guess1] [guess2] -- n )
	guessover guessover checkc >r
	0 \ correct so far
	1 fields 1+ -DO	
		swap i roll = +
	1 -LOOP
	negate
	r>
	over - ;

: curry ( x xt1 -- xt2 )
  swap 2>r :noname r> postpone literal r> compile, postpone ; ;
  
: times { xt n -- }
  n 0 ?do xt execute loop ;

: solcompare ( p1 c1 p2 c2 -- p1 c1 bool ) 
	2over rot = -rot = and ;
: issol ( p c -- p c b ) fields 0 solcompare ;
  
0 Value cnt  


: currycheck ( x x x x -- xt2 )  
\ call like this: 1 2 3 4 currychk is chk
 0 to cnt [ ' check ] literal [ ' curry ] literal fields times  ;

defer chk ( [guess] -- u u )
: checks ( x x x x -- u u )
	cnt 1 + dup ." Tries: " . cr to cnt chk issol IF cnt -1 + to cnt ELSE ENDIF ;

: sol! ( [guess] -- ) \ defines the solution
	currycheck is chk ; 


: prettyprint ( guessindex -- [guess] )
	dup cg
	." The solution is: " fields reverse fields 0 u+do . loop 
	cg ;
	
: ?? ( [guess] -- )
	checks 2dup
	swap
	." Pos: " . ." Col: " . 
	issol IF 
		." Solved! " 
	ELSE
	ENDIF 
	drop drop  ;

Variable seed

$10450405 Constant generator

: rnd  ( -- n )  seed @ generator um* drop 1+ dup seed ! ;
: random ( n -- 0..n-1 )  rnd um* nip ;
: rndsol ( -- [guess] ) nsol random cg ;
: init ( -- ) rndsol sol! ; 

init init init 

: reverse ( x_1 x_2 ... x_n-1 x_n n -- x_n x_n-1 ... x_2 x_1 )
 0 u+do i roll loop ;
\ put [1 ....... 1 ] in array 
\ guess something consistent , loop through the array and look what is consistent
\ repeat 

: shittyknuth ( -- [guess] )
	here nsol cells allot
	nsol  0 +DO
		-1 over i cells + !
	1 +LOOP 
	511 
	BEGIN
		nsol mod
		dup cg checks issol invert 
	WHILE
		nsol  0 +DO
			2 pick cg i cg check
			solcompare
			4 pick swap
			over i cells + @ 
			and swap i cells + !  \ updated memory
		1 +LOOP
		drop drop \ remove evalution of old solution
		drop ( remove old n )
		nsol 0 
		+DO
			dup i cells + @
			i swap 
			IF
				leave
			ELSE 
				drop 
			ENDIF
		1 +LOOP
	REPEAT 
	drop drop swap nsol negate cells allot drop
	prettyprint
;

\ get some values from the adress in the greatknuth function
: lastsol ( addr -- addr ) dup @ -1 + 2 * 1 + cells + ;
: lastres ( addr -- addr )  lastsol 1 cells + ;
: soladdrn ( addr n -- addr ) 2 * 1 + cells + ;
: resaddrn ( addr n -- addr ) 2 * 2 + cells + ;


\ encode and decode the response in one integer 
: encode ( a1 a2 -- a3 ) 4 lshift + ;
: decode ( a3 -- a1 a2 ) dup 1 4 lshift mod swap 4 rshift ;


\ these two functions are the same as those above but they dont leave the first response on the stack
: solcompar ( p1 c1 p2 c2 -- bool )  \ same as solcompare but discards p1 and c1
	rot = -rot = and ;
: isol ( p c -- p c b ) fields 0 solcompar ;

: greatknuth ( -- [guess] ) 
	here 40 cells allot 					\ array that contains the guesses and the response.
											\ 0th element is the current number of guesses so far
											\ than the odd numbers are the guesses
											\ even numbers are the responses encoded with encode (=> decode)
	0 										\ first guess harcoded because why not
	BEGIN
		nsol mod 							\ make sure that it's not a too big index
		over dup @ 1 + swap !  				\ increase counter by 1
		2dup swap lastsol !					\ save guess at correct position
		cg checks 							\ check guess
		encode 2dup swap lastsol 1 cells + ! decode \ save answer at lastsol+1cell
		isol invert 						\ check if its the correct solution and continue loop until this is the case
	WHILE
		nsol over lastsol @ 1 + +DO			\ loop from next guess to last guess
			dup @ 0 +DO
				dup i soladdrn @ cg			\ get ith guess
				j cg 		 				\ get next guess
				check 						\ check what would be the solution
				2 pick i resaddrn @ decode 	\ get answer of ith guess - should be consistent 
				solcompar invert 
				if 
					leave					\ if inconsistent to at least 1 previous solution leave
				endif
				\ if it came until here it is consistent if
				\ i is equal to the amount of tries so far -1
				dup @ -1 + i = if 
					-1234 leave 			\ magic number to signal outer loop what to do (fuckthis)
				endif
			1 +LOOP
			dup -1234 = if
				drop i leave				\ drop the magic number and leave this loop 
			endif 
		1 +LOOP
	REPEAT
	lastsol @
	prettyprint
;

: tk rl cr cr clearstack greatknuth ;  \ for debugging efficiently
