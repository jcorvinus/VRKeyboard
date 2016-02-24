# VR Hex Keyboard by JCorvinus

-----------------------------
Concept
-----------------------------
This is a demonstration of a touch-type virtual keyboard in VR, using Leap Motion hand tracking. Their tracking software was updated recently and I was curious to see if it was accurate enough to enable this kind of text input, since text input in VR is currently difficult. It isn't the most efficient thing. In testing I was able to get between 9 and 10 WPM, which is nothing like my 90 with a meatspace keyboard, but if you've only got your hands it's better than nothing.

-----------------------------
How to use
-----------------------------
First, make sure you've got a VR headset of some kind that supports Unity's native VR integration and ensure that you've set it up to operate. Then, ensure you've got a Leap Motion controller mounted to the front of the device. Download the Orion software: https://developer.leapmotion.com/orion and then:

If you want to build from source, grab the source code from here: https://github.com/jcorvinus/VRKeyboard and load it up in Unity (For the record this was built with Unity 5.3.2f1). From there just hit the play button to jump right in, or use the file->build option to build.

If you just want to run the demo, download from the release tab above, or download this file: https://mega.nz/#!Js0xzBQA!y8nRRNKTG60proe03v7KkkMVcOEW2AklVozF1cwjnbE extract it, then run VRKeyboard.exe. There is a menu attached to your left wrist that lets you change character sets and reset the HMD pose.

-----------------------------
Design
-----------------------------
The design of this keyboard is very deliberate. Most work went into the buttons themselves. I set out to solve the following problems:
- Ensuring that only the buttons targetted are the ones that are activated.
- Feedback on button selection
- Feedback on button throw distance
- Feedback on button activation

The first one, ensuring that the targetted button is the one that triggers, is first accomplished by the wide space between buttons. Also, the trigger volume is a capsule, but a very thin one. The fingertip itself is fairly wide, so a skinny collision volume allows the fingertip to still occlude it quite easily, but makes double-selection difficult. This, on top of the wide spacing pretty much guarantees it will never happen. Buttons also only respond to finger directions that are perpendicular to their face. This prevents a user from pushing their palm into the keyboard and slamming several keys at once.

For button selection, the user is given three channels of feedback: Glowing rays shoot out of the button's surface, and the outline of the button itself does a ping-pong style color tween from highlight to normal color. The last channel of feedback was actually a happy accident, due to the way the throw mechanism works. When you hover over the button, the face 'pops' to it's least-depressed position instantly.

Buttons have a property known as 'throw distance', and this is how far a button must be pressed inwards before activation. Here we give the user lots of feedback as well. When the fingertip is within range, the buttons' face will move to the position along the button axis that the fingertip lies on, and when it gets pushed past a certain distance, the button will activate. This distance is actually illustrated with a 'shadow' mesh that has the same outline as the button itself, but grey and opaque. Audio feedback is also given here. When the button press is initiated, a whining wave sound fades in and increases in pitch, letting the user know intuitively that there is a clear escalation to pushing the button further inwards. 

For the last one, when the button passes the threshold, two things happen: an activation sound is played, and the outline mesh of the button is cloned, and quickly scaled outwards.

-----------------------------
Usage / Contact
-----------------------------
Just about everything in the /VRKeyboard/ folder of the project was produced by myself and is licensed under the MIT license. Along with that, if you want to use this in your project all I ask is you mention me somewhere in it.
The /Environment/ folder was taken from Unity's GDC Mecanim demo. Sound effects come from the Sci-Fi-Sfx asset store package, and the UI_Buttons_Pack2 asset store package. Everything I didn't make is licensed under its own terms, so look into them before using.

To get ahold of me, follow me on Twitter at @JoshuaCorvinus, or visit my developer profile: https://jcorvinus.itch.io/
