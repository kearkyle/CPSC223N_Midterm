echo First remove old binary files
rm *.dll
rm *.exe

echo View the list of source files
ls -l

echo Compile Midtermui.cs to create the file: Midtermui.dll
mcs -target:library -r:System.Drawing.dll -r:System.Windows.Forms.dll -out:Midtermui.dll Midtermui.cs

echo Compile Midtermmain.cs and link the two previously created dll files to create an executable file. 
mcs -r:System -r:System.Windows.Forms -r:Midtermui.dll -out:Midterm.exe Midtermmain.cs

echo View the list of files in the current folder
ls -l

echo Run the Assignment 1 program.
./Midterm.exe

echo The script has terminated.
