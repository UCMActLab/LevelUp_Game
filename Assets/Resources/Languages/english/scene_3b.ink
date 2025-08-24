=== scene_3b_intro ===
~ news_count++
~ article_sent = false

Here's article number {news_count} 

TODO: WHO IS SENDING THE ARTICLE?

ARTICLE RECEIVED
Article headline: art05_headline # art05_multimedia

+ [Read the article] -> s3b_article
+ [Skip the article] -> s3b_choice

== s3b_article ==
~ news_read++
This article comes from art05_source.
art05_body -> s3b_choice

== s3b_choice == 
* [Send it to your friends.] 
~ article_forwarded_group1++
~ article_sent = true
-> s3b_g1

* [Send it to your family] 
~ article_forwarded_group2++
~ article_sent = true
-> s3b_g2

* [Send it to your neighbours] 
~ article_forwarded_group3++
~ article_sent = true
-> s3b_g3

* Don't send it to anyone{article_sent: else}. -> s3b_results

== s3b_g1 ==
Sent to Friends Group.
~ group_1_opinion(art05_theme, art05_true, art05_key)
{ art05_key == true: art05_ReactionG1}
-> s3b_choice
    
== s3b_g2 ==
Sent to Family Group.
~ group_2_opinion(art05_theme, art05_true, art05_key)
{ art05_key == true: art05_ReactionG2}
-> s3b_choice

== s3b_g3 == 
Sent to Neighbours Group.
~ group_3_opinion(art05_theme, art05_true, art05_key)
{ art05_key == true: art05_ReactionG3}
-> s3b_choice
    
== s3b_results == 

Friends points: {group_1}
Family points: {group_2}
Neighbors points: {group_3}

-> scene_4_intro
