: rl s" Mastermind.fs" included ; 
\ Usage:
\ >n cg
\ generates the n-th possible solution in the given system (depending on base and fields)
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

\ [guess] always refers to "fields" numbers ( in interval [0,base[ ) 

6 constant base
4 constant fields


: exp ( u1 u2 -- u3 ) \ u3 = u1^u2
   over swap 1 ?do over * loop nip ;
   
: nsol ( -- n ) [ base fields exp ] literal ;
: 4x4roll 4 roll 4 roll 4 roll 4 roll ;
: cg ( number  -- u*fields)
	nsol mod
	fields base rot
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
	base 1 +DO
		guessover guessover
	1 +LOOP
	0
	base 0 +DO
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
  
: currycheck ( x x x x -- xt2 )  
\ call like this: 1 2 3 4 currychk is chk
 [ ' check ] literal [ ' curry ] literal fields times  ;
 
defer chk 

: sol! ( [guess] -- ) \ defines the solution
	currycheck is chk ; 

: solcompare ( p1 c1 p2 c2 -- p1 c1 bool ) 
  2over rot = -rot = and ;

: issol ( p c -- p c b ) fields 0 solcompare ;

: ?? ( [guess] -- )
	chk 2dup
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

' init here nsol mod  times 



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
		dup cg chk issol invert 
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
	drop drop swap drop cg
	." The solution is: " swap 2swap swap 4x4roll . . . . 
;

: tk rl cr cr clearstack shittyknuth ;  \ for debugging efficiently