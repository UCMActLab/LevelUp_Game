Here the game tells the player how they're doing. 

TODO These variables are just for the prototype. We need a function to calculate the mood trends, but we need more items in the database for that. 

== game_ending ==
// Final reaction of group 1
~ temp group1_speaker = LIST_RANDOM(LIST_ALL(group1_members))
~ temp group2_speaker = LIST_RANDOM(LIST_ALL(group2_members))
~ temp group3_speaker = LIST_RANDOM(LIST_ALL(group3_members))

TODO IF THE PLAYER HAS NOT FORWARDED ANY NEWS, THE FAMILY SHOULD BE CHECKING ON THEM - AND OTHER GROUPS SHOULD NOTE THAT THE PLAYER HAS BEEN VERY QUIET.


{article_forwarded_group1 == 0: {group1_speaker}: You're a bit quiet.}
{article_forwarded_group2 == 0: {group2_speaker}: Are you okay?}
{article_forwarded_group3 == 0: {group3_speaker}: Haven't heard from you in a bit. }


{   
    - group_1 <= -2:
    {group1_speaker}: I don't think we see eye to eye on things. I don't trust your judgement. 
    - group_1 >= 2:
    {group1_speaker}: You get it. You're one of the good ones. 
  - else:
    {group1_speaker}: Hope everyone here is having a good day. 
}

// Final reaction of group 2
{   
    - group_2 <= -2:
    {group2_speaker}: We're really worried about you. You've been sending us information that is not reliable. 
    - group_2 >= 2:
    {group2_speaker}: We love hearing from you regularly. You seem engaged with the news in a healthy way. 
  - else:
    {group2_speaker}: When will we see you next. 
}

// Final reaction of group 3

{   
    - group_3 <= -2:
    {group3_speaker}: Please be mindful of what you send to us. We don't know what to believe from what you send. 
    - group_3 > 2:
    {group3_speaker}: Keep sending us articles, we have learned a lot. 
  - else:
    {group3_speaker}: It's a nice day out! We should all put away our phones. 
}

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


This is the end of the prototype. 

THANK YOU FOR PLAYING
-> END
