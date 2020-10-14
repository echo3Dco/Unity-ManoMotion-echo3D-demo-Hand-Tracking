Setup ManoMotion SDK Lite 1.3 with AR Foundations

*** Updated Features 1.3 ***

Improved hand tracking
Updated editor script (for iOS)

*** Updated Features 1.2 ***

Smoothing of gestures

***

To try out the capabilities and features of ManoMotion technology together with AR Foundations, navigate to ManoMotionAR Foundation Folder, Scenes and open the ManoMotion ARFoundation scene file. 

Switch the Unity project to the Android or iOs plattform.
Add the AR Foundations 2 and ARCore (Android) / ARKit (iOS) packages to your project from, Window -> Package Manager (more information https://docs.unity3d.com/Packages/com.unity.xr.arfoundation@2.2/manual/index.html)
AR Foundations does not support Vulkan Graphics API (This can be set in the Player settings).
Add the Scene to the build and build and run.
All of the necessary components for the ManoMotion Library to operate are already placed and set in the scene.

Common Issues & Solutions:
1)When importing the ManoMotion package, the prefabs appear to be missing.
    (Solution) Set your unity project to a version greater to 2018.3 
2)The application does not work and I am receiving a message of BundleIdentifier Missmatch.
    (Solution) Make sure that there are no spaces or additional characters in your license key(Check step 2) field or Bundle Identifier(Check step 4). 
3)The application does not work and I am receiving a message of Internet Required.
    (Solution) Make sure that the internet speed of your phone is adequate to communicate with our service. This issue is faced when there is either 
        a) No internet connection available in the device. 
        b) A very slow connection available in the device.

You can get more help, ideas and feedback by joining our development Discord server at https://discord.gg/5JYn9g 