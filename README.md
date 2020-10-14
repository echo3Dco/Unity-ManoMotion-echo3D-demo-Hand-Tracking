
# Unity-ManoMotion-echoAR-demo-Hand-Tracking
 
This Unity demo uses echoAR's 3D model streaming in combination with [ManoMotion](https://www.manomotion.com/), a framework for hand-tracking and gesture recognition in AR. Any number of models can be uploaded to the echoAR console and streamed into the app. You can tap on any detected horizontal plane to move the active model to that location, and use the button at the top of the screen to switch to the next model. The button can be tapped on via the screen, but it can also be used by placing your hand in front of the camera and making a "click" gesture behind the button, as shown below.

## Register
Don't have an API key? Make sure to register for FREE at [echoAR](https://console.echoar.xyz/#/auth/register).

## Setup
* Clone this project and open in Unity
* Set your echoAR API key in the echoAR prefab.
* Upload the contents of the [models folder](/models/) through the [echoAR console](https://console.echoar.xyz).
* Upload the metadata in the [metadata folder](/metadata/) for each model in the Data tab of the [echoAR console](https://console.echoar.xyz).

You can also add your own models to the echoAR console by searching or adding your own, and they will appear when you cycle through the models. Just make sure that the number of models does not exceed the value you give to the maxModels variable of the Place on Plane script of the AR Session Origin.

## Run
[Build and run the AR application](https://docs.echoar.xyz/unity/adding-ar-capabilities#4-build-and-run-the-ar-application).

## Learn more
Refer to our [documentation](https://docs.echoar.xyz/unity/) to learn more about how to use Unity and echoAR.

If you want more demos of Manomotion's technology, there are additional demos created by Manomotion in the Manomotion/Examples folder. Although these do not incorporate echoAR, they show off a lot more functionality. Please see [ManoMotion's website](https://www.manomotion.com/) to see documentation and to get your own API key.

## Support
Feel free to reach out at [support@echoAR.xyz](mailto:support@echoAR.xyz) or join our [support channel on Slack](https://join.slack.com/t/echoar/shared_invite/enQtNTg4NjI5NjM3OTc1LWU1M2M2MTNlNTM3NGY1YTUxYmY3ZDNjNTc3YjA5M2QyNGZiOTgzMjVmZWZmZmFjNGJjYTcxZjhhNzk3YjNhNjE).

## Screenshots
![demo](images/demo_video.gif)

Demo created by [Caleb Biddulph](https://github.com/CDBiddulph/).