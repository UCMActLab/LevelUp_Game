=== checkin_1 === 
~ temp group1_speaker = LIST_RANDOM(LIST_ALL(group1_members))
~ temp group2_speaker = LIST_RANDOM(LIST_ALL(group2_members))
~ temp group3_speaker = LIST_RANDOM(LIST_ALL(group3_members))

{article_forwarded_group1 == 0: {group1_speaker}: ^You're a bit quiet.}
{article_forwarded_group2 == 0: {group2_speaker}: Are you okay?}
{article_forwarded_group3 == 0: {group3_speaker}: Haven't seen you in a bit. }



Here the game tells the player how they're doing. 

TODO These variables are just for the prototype. We need a function to calculate the mood trends, but we need more items in the database for that. 
TODO Rather than reporting this in text, it'd be better to show it in a counter / bar on top of the screen, similar to what Reigns does. 

Your friends <>{
    - group_1 >= 1:
     are happy with you.
    - group_1 == 0:
     are indifferent. 
    - else:
    are not happy with you. 
}
Your family <>{
    - group_2 >= 1:
     seem to be happy about you.
    - group_2 == 0:
     are indifferent. 
    - else:
    are worried about you. 
}

Your neighbours <>{
    - group_3 >= 1:
     are happy with you.
    - group_3 == 0:
     are indifferent. 
    - else:
    are not happy with you. 
}

//The text below are the consequence for sending specific news to specific teams. This is hardcoded, so it can only be done with certain pieces of news. 
{s1_g3: Paula (neighbours): I'm on the phone with my bank. I used the link you send us to get compensation for the blackout, and someone stole my information. Don't use it, it's a scam! }
{s2_g1:
Mark (neighbours): I am not going to be able to join you for dinner tonight. I think I got covid when I went to the match a few days ago.
} 

-> scene_3_intro