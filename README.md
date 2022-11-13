# Image Processing Tool (Test Project)
This is a simple test project to enable various tests of the EMGU.CV (https://www.emgu.com/wiki/index.php/Main_Page) image processing library.

Currently this project leverages the following:
- .NET 4.6.2
- EMGU.CV 4.4.0 & runtime dlls
- Windows forms

The project is meant to be used in conjunction with anyother project that takes screenshots of your desktop or specific windows, the screenshot is then saved to the disk for later reference in the 'source_images' folder. This project has a 'reference_images' folder which contains images we took of specific images we wish to search for in the 'source' image.
The test simply provides a certainty level (percentage) of a match being found in the source image, it then marks it with a red rectangle and saves the resulting image as "Result.jpg" in the application working directory for later review if needed.

The idea of this test project is to use it as a playground/testing sandbox for additional image functionality, such as continuous image detection of specific applications in order to facilitate automation of some kind based off of images present on the screen.
