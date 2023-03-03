# Types
 - list     : ordered, heterogeneous collection
 - number   : integers or floats
 - char     : a char
 - string   : list of chars
 - function : lambda Functions
 - null     : the null value

# Import
```
import math // looks for a file called "math.fun" in the current directory, or a directory called "math" and imports the "main.fun" file from whitin
import "../math.fun"
```

# Variable definition
```
define a 5 // one number
define (a b c) (4.5 "hey" 6) // a bunch of numbers and strings
define l (3 4 5) // a list
define (l1 l2) ((2 3) (4 "a")) // a bunch of lists
```


# If block
```
if cond (+ 2 3) (- 4 5) // will return 5 or -1
if cond 4 5
```
An if expression always returns the last element of the list that it evaluated. For example, `if 1 (3 4) (9 0)` will return 4.

# Function definiton
```
lambda x => (* 2 x)
lambda (x y z) => (+ * x y z)
define f (lambda x => (1000 * 2 x))
```

A function always returns the last element of the list that it evaluated. For example, `f 4` will return 8, even though the list it evaluated turned out to be (1000 8).

## Function call
```
(f x)
f x
(f x y)
f x y
```

A function call can or cannot be surrounded by parantheses. As such, if you want to get a list with the result of `f x y`, you would need to do `((f x y))`, `list (f x y)` or `list f x y`.


Example:
```
(
define map lambda (f l) => (
if l
    (push (f first l) (map f (rest l)))
    (0)
)
map (lambda x => (+ 5 x)) (1 2 3 4)
)
```

## Functions
 - not e      : takes the logical value of the expression and negates it
 - push e l   : inserts e to the front of l and return the new list
 - pop l      : removes the first element of l and returns it, error if l is empty
 - println s  : prints s to stdout, followed by a new line
 - error e    : prints e to stderr and halts the program
 - ? input s  : reads string from stdin into s
 - first l    : returns the first element of l, error if l is empty
 - second l   : returns the second element of l, error if l has one element
 - third l    : returns the third element of l, error if l has two elements
 - rest l     : returns everything after the first element of l, error if l is empty
 - length l   : returns the length of l
 - empth l    : checks if l is empty
 - sum l      : sums over l
 - product l  : multiplies over l
 - map l f    : runs f for every element of l and returns the resulting list
 - filter l f : runs f for every element of l and returns a list containing the elements for each f is true

# Null
Some functions return the null value, such as `define` or `println`. This is so as to not pollute lists, because at evaluation, all null values are dropped, as well as al of the lists containing only null values.

# Comments
`/* comment */`
Ignores everything between the marks.

