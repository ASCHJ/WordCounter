# Table of Contents

1. [WordCounter CLI Application](#wordcounter-cli-application)   
    1.1. [Overview](#overview)   

    1.2. [Usage](#usage)   
    1.3. [Excluding of Words](#excluding-of-words)   
    1.4. [Expected Behavior](#expected-behavior)  

    1.5. [Execution Flow](#execution-flow)   
    1.5.1. [Producer/Consumer Concept](#producerconsumer-concept)   
    1.5.2. [Report Creation](#report-creation)   

# WordCounter CLI Application

## Overview

WordCounter is a command-line interface (CLI) application that generates catalogs detailing the number of words in a set of files. It operates without considering case sensitivity and provides the flexibility to exclude specific words from the catalogs. There will also be generated a dedicated report for the excluded words.

## Usage

To use WordCounter, follow the syntax below:

* --directory: Specifies the directory containing the files for word counting. Required.note

```bash
WordCounter.Cli.exe --directory "<path to test files>"
```

## Excluding of words
Excluding words from being cataloged is done by adding the 'exclude' words to the 'exlude.txt' file.   
An example of this can be seen below
```bash
sodales
Nunc
aliquet
ac
```

## Expected behavor
After program execution a catalog file is created for each letter in the alphabet. Each file contains the word and the count of its occurrences in the files.
```bash
FILE_A.txt
FILE_B.txt
..
..
FILE_Y.txt
FILE_Z.txt
```
The files will only contain the words that starts with the letter postfixed in the file name.   
For example, file 'FILE_A.txt" will only contain words that starts with the letter 'A'.
```bash
  AT 22
  ALIQUAM 16
  ..
  ..
  APTENT 1
  AMET 20
```

The generated exclude report 'ExcludeReport.txt' will contain the word and the occurrences of the excludes words.   
An example seen below:
```bash
QUISQUE 4
EU 19
..
..
ALIQUET 7
AC 22
```

# Execution Flow

A producer/consumer concept for reading and saving words to the repository is used. 
When completed, indexes of words are built for parallelization the creation and writing of the reports to disc.

## Producer/Consumer Concept

- **Producers**
  - Responsible for reading and serving lines from files to consumers.
  - The system can have 1 to N producers, where N is the number of files.

- **Consumers**
  - Responsible for parsing lines, counting different words per line, and saving words to the repository.

## Report Creation

To parallelize the creation of reports, indexes are built for two types of reports:

1. **Excluded Words**
   - One report is created for excluded words.

2. **Alphabetical Indexes**
   - 26 reports are created by the alphabetically letter.

All reports can be written to disc without affecting each other.
