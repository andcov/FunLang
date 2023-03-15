# FunLang
A Schema inspired, interpreted language written in C#.

# Types
 - list     : ordered, heterogeneous collection
 - number   : integer or float
 - char     : a char
 - string   : list of chars
 - function : lambda function

# Import

 - [ ] Implement imports

```
import math
```
Looks for a file called "math.fun" in the current directory, or a directory called "math" and imports the "main.fun" file from whitin.

```
import "../math.fun"
```
Imports the "../math.fun" file.

# Variable definition
```
define a 5 // one number
define (a b c) (4.5 "hey" 6) // a bunch of numbers and strings
define l (3 4 5) // a list
define (l1 l2) ((2 3) (4 "a")) // a bunch of lists
```

# If block
```
if (== 1 0) then (+ 2 3) else (- 4 5)
if 9 then 4 else 5
```
An if expression always returns the last element of the list that it evaluated. For example, `if 1 (3 4) (9 0)` will return 4.

# Comments
```
/* comment */
```
Ignores everything between the marks.

# Functions

## Function definiton
```
lambda x => (* 2 x)
lambda (x y z) => (+ * x y z)
define f lambda x => (1000 * 2 x)
```
In order to differentiate between function calls and the function itself, the `$` sign is used. As such, if you want to pass the function `f` as a parameter to another function, use `g $f`

## Function call
```
define f lambda x => (1000 * 2 x)
f 15
(f 15)
```

A function call can or cannot be surrounded by parantheses. If it is surrounded, it will not be considered a list. In order to create a list with the result of a function call, surround it twice: `((f x))`.

A function always returns the last element of the list that it evaluated. For example, `f 4` will return 8, even though the list it evaluated turned out to be `(1000 8)`.

Example:
```
(

define map lambda (f l) => (
	if l then (
		(push f first l map $f rest l)
	) else (
		()
	)
)

define filter lambda (f l) => (
	if l then (
		if (f first l) then (
			(push first l filter $f rest l)
		) else (
			(filter $f rest l)
		)
	) else (
		()
	)
)

println map $lambda x => (+ 5 x) filter $lambda x => (== 0 % x 2) (1 2 5 3 4 5 1 2 3 4 5 6 7 8 5 5 24)

)
```
This will output `(7 9 7 9 11 13 29)`

## List of functions

### Basic Operations

 - [x] `+ a b`
 - [x] `- a b`
 - [x] `* a b`
 - [x] `/ a b`
	- divides `a` by `b` and returns a float (regardless if `a` and `b` are integers)
 - [x] `% a b`
	- `a` and `b` must be integers

 - [x] `== a b`
 - [x] `!= a b`
 - [ ] `not a`
	- negates the logical value of `a`
 - [ ] `or a b`
	- logical or
 - [ ] `and a b`
	- logical and

For all operations (apart from modulo), if one of the numbers is a float and the other is an integer, the integer gets promoted to a float. If one of them is a char, it gets converted to its ASCII code.

### Standard functions
 - [x] `push e l`   : inserts e to the front of l and return the new list
 - [ ] `pop l`      : removes the first element of l and returns it, error if l is empty
 - [x] `println s`  : prints s to stdout, followed by a new line
 - [ ] `error e`    : prints e to stderr and halts the program
 - [ ] `? input s`  : reads string from stdin into s
 - [x] `first l`    : returns the first element of l, error if l is empty
 - [x] `second l`   : returns the second element of l, error if l has one element
 - [ ] `third l`    : returns the third element of l, error if l has two elements
 - [x] `rest l`     : returns everything after the first element of l, error if l is empty
 - [x] `length l`   : returns the length of l
 - [ ] `empty l`    : checks if l is empty
 - [ ] `sum l`      : sums over l
 - [ ] `product l`  : multiplies over l
 - [ ] `map l f`    : runs f for every element of l and returns the resulting list
 - [ ] `filter l f` : runs f for every element of l and returns a list containing the elements for each f is true

# TODO
 - [ ] reformat code
 - [ ] make error reporting work with tabs
 - [ ] fix line counting when reporting errors
 - [ ] add support for special chars ('\n')
 - [ ] implement logical value for Expression

