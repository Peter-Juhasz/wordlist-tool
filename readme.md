# Wordlist Tool
A chainable, high-performance tool for manipulating wordlists. Wordlists can be used for many purposes: word games, allow or deny lists, even for password cracking.


## Usage
```
Usage:
  wl [command] [options]

Options:
  --encoding <encoding>        Encoding of the wordlist file. [default: ASCII]
  --line-ending <line-ending>  Line ending sequence. [default: 0A]
  --version                    Show version information
  -?, -h, --help               Show help and usage information

Commands:
  sort            Sort entries.
  filter          Filter entries.
  transform       Transform entries.
  list            Transform list.
  extract         Extract entries from other formats.
  merge           Merge entries from multiple lists.
  generate        Generate entries.
```

For example:
```ps
wl extract --inputs books/*.txt --output OUT --regex \w+ |
wl filter distinct IN OUT |
wl sort asc IN OUT |
wl transform-list take IN output.txt 1000
```

### Sort
You can sort entries in ascending order the following way:
```ps
wl sort asc list.txt
```

Output can be saved to another file:
```ps
wl sort asc list.txt output.txt
```

Sort entries in a descending way:
```ps
wl sort desc list.txt
```

Reverse sort order:
```ps
wl sort reverse list.txt
```


### Filter
Remove whitespace entries:
```ps
wl filter whitespace list.txt output.txt
```

Remove duplicates
```ps
wl filter distinct list.txt output.txt
```

Filter entries which match (contain) a regular expression:
```ps
wl filter regex list.txt output.txt \d+
```

Filter entries by a regular expression by full match:
```ps
wl filter regex list.txt output.txt \A\d+\Z
```

Filter entries by minimum length:
```ps
wl filter min-length list.txt output.txt --length 3
```

Filter entries by maximum length:
```ps
wl filter max-length list.txt output.txt --length 3
```


### Transform
Trim leading and trailing whitespace:
```ps
wl transform trim list.txt output.txt
```

Convert entries to uppercase:
```ps
wl transform uppercase list.txt output.txt
```

Convert entries to lowercase:
```ps
wl transform lowercase list.txt output.txt
```

Prepend to entries:
```ps
wl transform prepend list.txt output.txt --value "prefix"
```

Append to entries:
```ps
wl transform append list.txt output.txt --value "suffix"
```

Reverse entries:
```ps
wl transform reverse list.txt output.txt
```

No not change entries. This transform is useful to either display:
```ps
wl transform identity list.txt OUT
```

Or change encoding or line endings of a list:
```ps
wl transform identity in.txt out.txt --input-encoding UTF8 --output-encoding ASCII
```


### Merge
Concatenate multiple lists together:
```ps
wl merge concat --inputs list1.txt list-*.txt --output output.txt
```

Union of multiple lists:
```ps
wl merge union --inputs list1.txt list-*.txt --output output.txt
```

Combine/zip together multiple lists line by line with no separator:
```ps
wl merge zip --inputs list1.txt list2.txt --output output.txt
```

Combine/zip together multiple lists line by line with a separator:
```ps
wl merge zip --inputs list1.txt list2.txt --output output.txt --separator ":"
```

Combine each line with each other line with no separator:
```ps
wl merge cross --inputs list1.txt list2.txt --output output.txt
```

Combine each line with each other line with a separator:
```ps
wl merge cross --inputs list1.txt list2.txt --output output.txt --separator ":"
```

Combine each line with each other line of itself with a separator:
```ps
wl merge cross --inputs single.txt --output output.txt --separator ":"
```

Remove entries which can be found in other lists:
```ps
wl merge except --inputs list.txt except-these.txt and-these-*.txt --output output.txt
```


### List operations
Take first N entries:
```ps
wl transform-list take list.txt output.txt --count 500
```

Take last N entries:
```ps
wl transform-list take-last list.txt output.txt --count 500
```

Skip first N entries:
```ps
wl transform-list skip list.txt output.txt --count 500
```

Skip last N entries:
```ps
wl transform-list skip-last list.txt output.txt --count 500
```


### Extract
Extract words from files using a regular expression (`\w+` by default):
```ps
wl extract regex --inputs books/*.txt --output output.txt 
```

Using a custom regular expression:
```ps
wl extract regex --inputs books/*.txt --output output.txt --regex [a-z]+
```


### Generate (WIP)
Generate entries:
```ps
wl generate new output.txt --charset 0123456789 --min-length 1 --max-length 5
```


## Configuration

### Input bindings cardinality
Wordlist Tool tries to process lists by streaming to avoid locking resources and buffering whole lists into memory (whenever possible).
This is why in general both an input and output path must be provided:
```ps
wl transform trim intput.txt output.txt
```

In some special cases, where buffering is inevitable (e.g. sorting, distinct), the same path can be used:
```ps
wl sort asc file.txt
```

Some operations support working with multiple inputs (merge, extract). You can either specify multiple files by their path:
```
wl merge union --inputs file1.txt file2.txt --output out.txt
```

Or use glob patterns:
```
wl merge union --inputs first.txt file-*.txt last.txt --output out.txt
```

*Note: patterns are evaluated individually, and if multiple patterns match the same file, it is going to be included multiple times.*

### Standard Input/Output bindings and chaining
You can use the reserved word `IN` and `OUT` to bind either input or output to standard input/output:
```ps
wl transform lower file.txt OUT
```

This makes it possible to chain commands together:
```ps
wl transform lower file.txt OUT |
wl filter distinct IN OUT |
wl sort asc IN final.txt
```

### Encoding
Encoding can be specified, default is `ASCII`:
```ps
wl transform lower in.txt out.txt --input-encoding UTF-8 --output-encoding ASCII
```

You can use the `identity` transform to change encoding of a list:
```ps
wl transform identity in.txt out.txt --input-encoding UTF8 --output-encoding ASCII
```

### Line endings
Line endings can be specified in HEX notation, default is `0A`:
```ps
wl transform lower in.txt out.txt --input-line-ending 0D0A --output-line-ending 0A
```

You can use the `identity` transform to change line endings of a list:
```ps
wl transform identity in.txt out.txt --input-line-ending 0D0A --output-line-ending 0A
```

### Buffering
Read and write buffering can be specified the following way:
```ps
wl transform lower in.txt out.txt --input-buffer-size 4096 --output-buffer-size 16384
```

