=== scene_3_intro ===
~ news_count++
~ article_sent = false

Here's article number {news_count}. 
TODO: WHO IS SENDING THE ARTICLE?

ARTICLE RECEIVED
Article headline: art04_headline # art04_multimedia

+ [Read the article] -> s3_article
+ [Skip the article] -> s3_choice

== s3_article ==
~ news_read++
This article comes from art04_source.
art04_body-> s3_choice

== s3_choice == 
* [Send it to your friends.] 
~ article_forwarded_group1++
~ article_sent = true
 -> s3_g1
    
* [Send it to your family.] 
~ article_forwarded_group2++
~ article_sent = true
 -> s3_g2
    
* [Send it to your neighbours.] 
~ article_forwarded_group3++
~ article_sent = true
-> s3_g3
    
* [Don't send it to anyone{article_sent: else}.] -> s3_results

== s3_g1 ==
Sent to Friends Group.
~ group_1_opinion(art04_theme, art04_true, art04_key)
{ art04_key == true: art04_ReactionG1}
-> s3_choice
    
== s3_g2 ==
Sent to Family Group.
~ group_2_opinion(art04_theme, art04_true, art04_key)
{ art04_key == true: art04_ReactionG2}
-> s3_choice

== s3_g3 == 
Sent to Neighbours Group.
~ group_3_opinion(art04_theme, art04_true, art04_key)
{ art04_key == true: art04_ReactionG3}
 -> s3_choice

== s3_results == 

Friends points: {group_1}
Family points: {group_2}
Neighbors points: {group_3}

-> scene_3b_intro
