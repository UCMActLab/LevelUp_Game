=== scene_0_intro ===
~ news_count++
~ article_sent = false

Here's article number {news_count} 

TODO: WHO IS SENDING THE ARTICLE?

ARTICLE RECEIVED
Article headline: art0_headline # art0_multimedia

+ [Read the article] -> s0_article
+ [Skip the article] -> s0_choice

== s0_article ==
~ news_read++
This article comes from art0_source.
art0_body -> s0_choice

== s0_choice == 
* [Send it to your friends.] 
~ article_forwarded_group1++
~ article_sent = true
-> s0_g1

* [Send it to your family] 
~ article_forwarded_group2++
~ article_sent = true

-> s0_g2
* [Send it to your neighbours] 
~ article_forwarded_group3++
~ article_sent = true

-> s0_g3
* Don't send it to anyone {article_sent: else}. -> s0_results

== s0_g1 ==
Sent to Friends Group.
~ group_1_opinion(art0_theme, art0_true, art0_key)
{ art0_key == true: art0_ReactionG1}
-> s0_choice
    
== s0_g2 ==
Sent to Family Group.
~ group_2_opinion(art0_theme, art0_true, art0_key)
{ art0_key == true: art0_ReactionG2}
-> s0_choice

== s0_g3 == 
Sent to Neighbours Group.
~ group_3_opinion(art0_theme, art0_true, art0_key)
{ art0_key == true: art0_ReactionG3}
-> s0_choice
    
== s0_results == 

Friends points: {group_1}
Family points: {group_2}
Neighbors points: {group_3}

-> scene_1_intro
