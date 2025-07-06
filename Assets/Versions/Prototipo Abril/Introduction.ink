// This is an example of how the game would start 

== Introduction ==
This scene is a basic tutorial for the player. 

The player will see that they've received a piece of news here - they can read the headline. 

"THIS IS THE HEADLINE OF A FAKE PIECE OF NEWS"

Character 1: Hey, have a look at this. 

+ [Read the article] -> article
+ [Skip the article] -> opinion

== article ==
This is the article text. When you read it critically, you can see that this is not real. -> opinion

== opinion ==
...
Character 1: What do you think? 

+ [I think this is garbage.] -> explanation
+ [This is very interesting.] -> explanation
+ [I don't know what to think about this.]-> explanation

== explanation ==
Character 1:"{article: Good on you for reading the article!}{not article:How can you have an opinion if you didn't read the article?}"
Character 1: "In this game, you're going to receive pieces of news, just like in real life. It's up to you to send them to other people."


This is where we tell the player what they have to do. 

Do you want to learn who the groups are?

+ [Yes.] You're going to send your game to three different groups.
-> explanation_groups
+ [No, I already know.] -> scene_1_intro

== explanation_groups ==

* [Who is group 1?] -> explain_g1
* [Who is group 2?] -> explain_g2
* [Who is group 3?] -> explain_g3
* -> scene_1_intro // this is not an option, it just means that once all the groups have been described the game will move to the next knot. 

== explain_g1 ==
This is group 1.
They believe news about disinformation. 
They do not like science. 
They believe conspiracy theories.
They do not like artificial intelligence. 
They often fall from scams. -> explanation_groups
== explain_g2 ==
This is group 2
They are very critical of things that sound like disinformation. 
They believe in science. 
They get angry at conspiracy theories.
They are very skeptical about artificial intelligence. 
They dislike anything that sounds like a scams. -> explanation_groups
== explain_g3 ==
This is group 3 
They often fall for disinformation. 
They like science, but it is scary. 
They believe conspiracy theories.
They think artificial intelligence is scary. 
They can fall from scams. -> explanation_groups
