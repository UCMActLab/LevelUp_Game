=== checkin_2 ===
~ temp group1_speaker = LIST_RANDOM(LIST_ALL(group1_members))
~ temp group2_speaker = LIST_RANDOM(LIST_ALL(group2_members))
~ temp group3_speaker = LIST_RANDOM(LIST_ALL(group3_members))

{article_forwarded_group1 == 0: {group1_speaker}: You're a bit quiet.}
{article_forwarded_group2 == 0: {group2_speaker}: Are you okay?}
{article_forwarded_group3 == 0: {group3_speaker}: Haven't seen you in a bit. }


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

{s3_g3: Omar: I want to thank you for sending that article about immigration the other day. It helped my friend get an appointment so she can work legally. She's very relieved.}

TODO Fix the conditional below, so the story reflects whether the mask articles have been sent or not. 
{ s4_g1: Mariah: Mark is in the hospital now. He took the medicine in the article you sent because he wanted to feel better fast, but he took too much and got a terrible rash. Doctors hare keeping him in tonight.}


-> scene_5_intro
