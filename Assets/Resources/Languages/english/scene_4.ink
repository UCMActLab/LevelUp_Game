=== scene_4_intro ===
~ news_count++
~ article_sent = false

Here's article number {news_count}. 
TODO: WHO IS SENDING THE ARTICLE?

ARTICLE RECEIVED
Article headline: art06_headline # art06_multimedia

+ [Read the article] -> s4_article
+ [Skip the article] -> s4_choice

== s4_article ==
~ news_read++
This article comes from art06_source.
art06_body-> s4_choice

== s4_choice == 
* [Send it to your friends.] 
~ article_forwarded_group1++
~ article_sent = true
-> s4_g1
    
* [Send it to your family.] 
~ article_forwarded_group2++
~ article_sent = true
 -> s4_g2
 
* [Send it to your neighbours.] 
~ article_forwarded_group3++
~ article_sent = true
  -> s4_g3
  
* [Don't send it to anyone{article_sent: else}.] -> s4_results

== s4_g1 ==
Sent to Friends Group.
~ group_1_opinion(art06_theme, art06_true, art06_key)
{ art06_key == true: art06_ReactionG1}
-> s4_choice

== s4_g2 ==
Sent to Family Group.
~ group_2_opinion(art06_theme, art06_true, art06_key)
{ art06_key == true: art06_ReactionG2}
-> s4_choice

== s4_g3 == 
Sent to Neighbours Group.
~ group_3_opinion(art06_theme, art06_true, art06_key)
{ art06_key == true: art06_ReactionG3}
-> s4_choice


== s4_results == 

Friends points: {group_1}
Family points: {group_2}
Neighbors points: {group_3}

-> checkin_2