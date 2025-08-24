=== scene_1b_intro ===
~ news_count++
~ article_sent = false

Here's article number {news_count} 

TODO: WHO IS SENDING THE ARTICLE?

ARTICLE RECEIVED
Article headline: art02_headline # art02_multimedia

+ [Read the article] -> s1b_article
+ [Skip the article] -> s1b_choice

== s1b_article ==
~ news_read++
This article comes from art02_source.
art02_body -> s1b_choice

== s1b_choice == 
* [Send it to your friends.] 
~ article_forwarded_group1++
~ article_sent = true
-> s1b_g1

* [Send it to your family] 
~ article_forwarded_group2++
~ article_sent = true

-> s1b_g2
* [Send it to your neighbours] 
~ article_forwarded_group3++
~ article_sent = true

-> s1b_g3
* Don't send it to anyone{article_sent: else}. -> s1b_results

== s1b_g1 ==
Sent to Friends Group.
~ group_1_opinion(art02_theme, art02_true, art02_key)
{ art02_key == true: art02_ReactionG1}
-> s1b_choice
    
== s1b_g2 ==
Sent to Family Group. 
~ group_2_opinion(art02_theme, art02_true, art02_key)
{ art02_key == true: art02_ReactionG2}
-> s1b_choice

== s1b_g3 == 
Sent to Neighbours Group.
~ group_3_opinion(art02_theme, art02_true, art02_key)
{ art02_key == true: art02_ReactionG3}
-> s1b_choice
    
== s1b_results == 

Friends points: {group_1}
Family points: {group_2}
Neighbors points: {group_3}

-> scene_2_intro
