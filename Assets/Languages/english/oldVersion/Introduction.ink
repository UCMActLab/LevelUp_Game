// This is an example of how the game would start 

== Introduction ==

This would be a basic tutorial for the player. 

The player will see that they've received a piece of news here - maybe just the headline. 

"THIS IS THE HEADLINE OF A FAKE PIECE OF NEWS"

Character 1: Hey, have a look at this. 

+ [Read the article] -> article
+ [Skip the article] -> opinion

== article ==
This is the article text. When you read it critically, you can see that this is not real. 
-> opinion

== opinion ==
Character 1: What do you think? 

+ [I think this is garbage.] -> explanation
+ [This is very interesting.] -> explanation
+ [I don't know what to think about this.] -> explanation

== explanation ==
Character 1:{ article: Good on you for reading the article!}{not article: How can you have an opinion if you didn't read the article?}
This headline was a test. 

This is where we tell the player what they have to do. 

Now let's put you to the test. 

-> scene_1_intro