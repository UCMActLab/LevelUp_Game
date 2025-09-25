=== scene_5_intro ===
~ news_count++
~ article_sent = false

Here's article number {news_count}. 
TODO: WHO IS SENDING THE ARTICLE?

ARTICLE RECEIVED
Article headline: art07_headline # art07_multimedia

+ [Read the article] -> s5_article
+ [Skip the article] -> s5_choice

== s5_article ==
~ news_read++
This article comes from art07_source.
art07_body-> s5_choice

== s5_choice == 

* [Send it to your friends.] 
~ article_forwarded_group1++
~ article_sent = true

  -> s5_g1
* [Send it to your family] 
~ article_forwarded_group2++
~ article_sent = true

   -> s5_g2
* [Send it to your neighbours] 
~ article_forwarded_group3++
~ article_sent = true

   -> s5_g3
* [Don't send it to anyone{article_sent: else}.] -> s5_results

== s5_g1 ==
Sent to Friends Group.
~ group_1_opinion(art07_theme, art07_true, art07_key)
{ art07_key == true: art07_ReactionG1}
-> s5_choice
    
== s5_g2 ==
Sent to Family Group.
~ group_2_opinion(art07_theme, art07_true, art07_key)
{ art07_key == true: art07_ReactionG2}
-> s5_choice

== s5_g3 == 
Sent to Neighbours Group.
~ group_3_opinion(art07_theme, art07_true, art07_key)
{ art07_key == true: art07_ReactionG3}
-> s5_choice


== s5_results == 

Friends points: {group_1}
Family points: {group_2}
Neighbors points: {group_3}

-> game_ending
