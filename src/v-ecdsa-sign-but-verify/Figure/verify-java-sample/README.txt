This program can be used to verify digital signatures from production.  The program takes two arguments:

- an input file with the JSON response to validate
- the x-clearcapital-signature response header value

The program will then read the file and the production public key(the
public.pem file in this directory) and validate the signature value.  A
response of "true" means the signature is valid.

Two same production input files are also included here; you can use them to
validate the signatures below:

java verify input_clearprop.json MEUCIFCHc7ipel34M2bVxt/OhHTrPPvl1sYKrBc0ylwpV2NAAiEA0caNs9DjhgO7sI4/6NmxD4n39t8zgfElQ8LM1ciF+pY=
java verify input_avm.json MEQCIE1zYJVW6PgCEyE6tlUtoH9k7BvsSIch/dwyS4HUhkVLAiBhK/Be0O2Rs1hdwylOWqv0I50sZ0l1O66pWqfHtR8aKw==

