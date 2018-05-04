osu! Metadata Anonymizer v. 1.0 - by user J1NX1337
Default noun and adjective text data copied from https://github.com/nekodex/namegen

How To Use

NOTE: This version of the anonymizer only supports pure .osu files. Hitsound sampleset randomization not included.

1. Select the .osu files you want to anonymize. The discrepancy detector will highlight any inconsistencies in metadata between
the .osu files. Orange color means there is a discrepancy in at least one of the selected files. Green means that all the metadata
of said type matches between the files. You may add .osu files to anonymize with the "Add" button, and remove a selected map with the
"Remove" button. It is also possible to edit any metadata field by double clicking its respective cell in the window. All selected
files are copies, and do not affect the original files in any way.

2. Select the adjective and noun lists you want to generate random entry names with. The lists themselves are plain text files
containing adjectives and nouns respectively, separated by new lines. You're free to include your own custom word lists. They will
be read from their respective folders upon startup, or can be added via the "..." option from a remote destination while the program
is running.

3. Click "Anonymize Metadata". Anonymization changes the Version metadata to a random adjective and noun combination, using the selected
word lists. It will then generate anonymized copies of the selected .osu files in the Output folder, with anonymized filenames as well.
If there are less words in the files than selected beatmaps, the program will inform of this.
The program will also disallow creating duplicate keys, so make sure your word lists don't both contain copies of each others' words.
A key containing the original Version metadata, as well as the new randomized Version metadata, will be generated in the program folder.