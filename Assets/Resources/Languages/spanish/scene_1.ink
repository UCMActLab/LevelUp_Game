=== scene_1_intro ===
TODO: DO WE WANT TO INCLUDE WHO IS SENDING THE ARTICLE

~ news_count++
~ article_sent = false
Here's article number {news_count} 

TODO The line is the line that selects the article from the database. This should hook into the external database. 


ARTICLE RECEIVED
Article headline : art01_headline # art01_multimedia
TODO: These values need to be taken from the database - this will be tags for each piece of news. The database also needs at least a headline and a text body. This code will be the same in each scene. 

+ [Read the article] -> s1_article
+ [Skip the article] -> s1_choice

=== s1_article ===
~ news_read++
This article comes from art01_source.
art01_body -> s1_choice

== s1_choice == 
TODO:  Still need to fix the last option so the game moves to the next knot when all options have been exhausted. I fixed the last option, so it checks whether the article has been sent already or not and displays the right text, but this may have issues when translating the game to other languages -- it's not good practice for localization. 

* [Send it to your friends.] 
    ~ article_forwarded_group1++
    ~ article_sent = true
   -> s1_g1

* [Send it to your family.] 
    ~ article_forwarded_group2++
    ~ article_sent = true
    -> s1_g2
    
* [Send it to your neighbours.] 
    ~ article_forwarded_group3++
    ~ article_sent = true
    -> s1_g3
    
* [Don't send it to anyone{article_sent: else}.] -> s1_results
    
== s1_g1 ==
Sent to Friends Group.
~ group_1_opinion(art01_theme, art01_true, art01_key)
{ art01_key == true: art01_ReactionG1}
-> s1_choice
    
== s1_g2 ==
Sent to Family Group.
~ group_2_opinion(art01_theme, art01_true, art01_key)
{ art01_key == true: art01_ReactionG2}
-> s1_choice

== s1_g3 == 
Sent to Neighbours Group.
~ group_3_opinion(art01_theme, art01_true, art01_key)
{ art01_key == true: art01_ReactionG3}
-> s1_choice

== s1_results == 

Friends points: {group_1}
Family points: {group_2}
Neighbors points: {group_3}

-> scene_1b_intro
