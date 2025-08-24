=== scene_2_intro ===
~ news_count++
~ article_sent = false

Here's article number {news_count} 

TODO: WHO IS SENDING THE ARTICLE?

ARTICLE RECEIVED
Article headline: art03_headline # art03_multimedia

+ [Read the article] -> s2_article
+ [Skip the article] -> s2_choice

== s2_article ==
~ news_read++
This article comes from art03_source.
art03_body -> s2_choice

== s2_choice == 
* [Send it to your friends.]
~ article_forwarded_group1++
~ article_sent = true
-> s2_g1

* [Send it to your family]
~ article_forwarded_group2++
~ article_sent = true

-> s2_g2
* [Send it to your neighbours] 
~ article_forwarded_group3++
~ article_sent = true

-> s2_g3
* [Don't send it to anyone{article_sent: else}.] -> s2_results

== s2_g1 ==
Sent to Friends Group.
~ group_1_opinion(art03_theme, art03_true, art03_key)
{ art03_key == true: art03_ReactionG1} 
-> s2_choice
    
== s2_g2 ==
Sent to Family Group.
~ group_2_opinion(art03_theme, art03_true, art03_key)
{ art03_key == true: art03_ReactionG2}
-> s2_choice

== s2_g3 == 
Sent to Neighbours Group.
~ group_3_opinion(art03_theme, art03_true, art03_key)
{ art03_key == true: art03_ReactionG3}
-> s2_choice
    
== s2_results == 

Friends points: {group_1}
Family points: {group_2}
Neighbors points: {group_3}

-> checkin_1
