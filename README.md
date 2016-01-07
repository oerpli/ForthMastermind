# ForthMastermind
Mastermind implementation in Forth.

### Usage
* Enter init to generate a new solution and then play.
* Put numbers on the stack and then enter `??` to test the solution. Outputs number of correct colors (col) and correct color on correct position (pos) 
* Use `shittyknuth` if you're to dense to solve it on your own. This runs an inefficient version of Knuth's mastermind algorithm and outputs the solution. If you're in time trouble use `greatknuth` instead as it's much faster!
 
#### Example usage
    init
    0 2 3 1 ??
    0 4 2 0 ??
    shittyknuth 
    ." Look mommy, I solved it!" cr
    ." Hurry up Jimmy, grandma is waiting!" cr 
    init
    greatknuth
    ." I solved it even faster this time!" cr


### Further Notes
* Number of possible colors (decoded as colors starting from 0) and fields (number of pins to guess) can be set to whatever you want. We recommend to stay below 7 fields though - it soon gets slow. 
* This note is here to justify the plural in the title of this section.

## License 
The MIT License (MIT)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

If the Author of the Software (the "Author") needs a place to crash and you have a sofa available, you should maybe give the Author a break and let him sleep on your couch.

If you are caught in a dire situation wherein you only have enough time to save one person out of a group, and the Author is a member of that group, you must save the Author.

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.


THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
