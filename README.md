# ForthMastermind
Mastermind implementation in Forth.

### Usage
* Enter init to generate a new solution and then play.
* Put numbers on the stack and then enter `??` to test the solution. Outputs number of correct colors (col) and correct color on correct position (pos) 
* Use `shittyknuth` if you're to dense to solve it on your own. This runs an inefficient version of Knuth's mastermind algorithm and outputs the solution.
 
#### Example 
    init
    0 2 3 1 ??
    0 4 2 0 ??
    shittyknuth 
    ." Look mommy, I solved it!" cr


### Further Notes
* Number of possible colors (decoded as colors starting from 0) and fields (number of pins to guess) can be set to whatever you want.
* This note is here to justify the plural in the title of this section.
