# hieroconv
Hieroconv is a console AngelCode bitmap font format converter. 
It's used to convert .fnt files in plain text format to .fnt XML tagged ones.
Created as a workaround for current MonoGame framework lack of integrated pipeline for bitmap fonts.
To be used in conjuction with XML .fnt parsing techniques.

# usage
====================================================================
"hieroconv [file path] [-p (for preview without any writing)]".
Just grab hieroconv.exe from bin/debug folder, stick it anywhere and use through command prompt.
Example: you have ubuntu.fnt plain font file in your C: drive root directory.
Open console wherever you placed hieroconv.exe and type "hieroconv C:/ubuntu.fnt" to convert it.
New file ubuntu_xml.fnt will be created in same directory as initial file.
Alternatively you can use "hieroconv c:/ubuntu.fnt -p" to view the result without actually converting or writing the file.
