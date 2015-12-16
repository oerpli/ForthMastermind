: rl s" test.fs" included ; 

6 constant base
4 constant fields

: g ( -- [guess] ) \ for testing purposes generates guess [ 0 .. fields]
	fields  0  +DO 
		i
	1 +LOOP ;

: guessover ( [guess1] [guess2] -- [guess1] [guess2] [guess1] )
	fields 0 +DO
		fields 2 * -1 + pick
	1 +LOOP
;

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
	over - 
;

: curry ( x xt1 -- xt2 )
  swap 2>r :noname r> postpone literal r> compile, postpone ; ;
  
: times { xt n -- }
  n 0 ?do xt execute loop ;
  
: currycheck ( x x x x -- xt2 )  
\ call like this: 1 2 3 4 currychk is chk
 [ ' check ] literal [ ' curry ] literal fields times  ;
 
defer chk 


: exp ( u1 u2 -- u3 ) \ u3 = u1^u2
   over swap 1 ?do over * loop nip ;


: 4x4roll 4 roll 4 roll 4 roll 4 roll ;

: createguess ( number  -- u*fields fields base)
	fields base rot
	0 3 pick 1 -
	-DO
		over i exp
		2dup /
		4x4roll
		mod
	1 -LOOP  
	-rot drop drop ;