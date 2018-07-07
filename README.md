# LiveSplit.CrashNSTLoadRemoval
LiveSplit component to automatically detect and remove loads from the Crash N Sane Trilogy.

This is adapted from my standalone detection tool https://github.com/thomasneff/CrashNSaneTrilogyLoadDetector
and from https://github.com/Maschell/LiveSplit.PokemonRedBlue for the base component code.

# Special Thanks
Special thanks go to McCrodi from the Crash Speedrunning Discord, who helped me by providing 1080p/720p captured data and general feedback regarding the functionality.

# How does it work?
The method works by taking a small "screenshot" (currently 300x100) from your selected capture at the center, where "LOADING" is displayed when playing the Crash NST. It then cuts this 300x100 image into patches (currently of size 50x50). From these patches, a color histogram is computed (currently using 16 histogram bins -> [0-15, 16-31, 32-47, ..., 240-255]) of the red, green and blue color channels. These histograms are put into a large vector, which describes our image (feature vector).

To detect if a screen is "LOADING" or not, we compute this feature vector every ~4-16ms (depending on capture modes, fast enough for real-time load detection) and compare it to a precomputed list of feature vectors. This list has currently been precomputed for the english version of the NST using different VODs and Remote Play footage. The precomputed vectors are simply snapshots during the "LOADING" screen (also during animation, when Aku Aku flies over "LOADING", different quality settings...).
We detect a "LOADING" screen if our current feature vector has similar enough histogram bins to any of the precomputed vectors. Comparing against multiple vectors allows for more robust detection in settings where "LOADING" is partially occluded or different video quality settings.

I decided to go for this simplistic approach (rather than e.g. computing SIFT features, histogram of gradients, deep learning detection...) as it doesn't have any external dependencies (which e.g. deep learning would have) and allows for real-time detection.

# Missing Features
If you do full trilogy run, you'll still need to time your title screen loads. Sorry, that's just because the title screen loads are different than the ingame loads. I might tinker around with also detecting those, but since there are so few of them, I'm not sure if that would be worth it.

# Settings
The LiveSplit.CrashNSTLoadRemoval.dll goes into your "Components" folder in your LiveSplit folder.

Add this to LiveSplit by going into your Layout Editor -> Add -> Control -> CrashNSTLoadRemoval.

You can specify to capture either the full primary Display (default) or an open window. This window has to be open (not minimized) but does not have to be in the foreground.

This might not work for windows with DirectX/OpenGL surfaces, nothing I can do about that. (Use Display capture for those cases, sorry, although even that might not work in some cases). In those cases, you will probably get a black image in the capture preview in the component settings.

