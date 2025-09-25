=== scene_3b_intro ===
~ news_count++
~ article_sent = false

~ temp print_article = LIST_RANDOM(LIST_ALL(articles)) // This lines selects an article at random
~ theme = article_data (print_article, Theme)
~ checked = article_data(print_article, verified)
Here's article number {news_count} 

TODO: WHO IS SENDING THE ARTICLE?

ARTICLE RECEIVED
Article headline: {article_data (print_article, headline)}

+ [Read the article] -> s3b_article
+ [Skip the article] -> s3b_choice

== s3b_article ==
~ news_read++
{display_source()}
// The body of the text goes below.
{article_data (print_article, body)} -> s3b_choice

== s3b_choice == 
* [Send it to your friends.] 
~ group_1_opinion(theme, checked)
~ article_forwarded_group1++
~ article_sent = true
-> s3b_g1

* [Send it to your family] 
~ group_2_opinion(theme, checked)
~ article_forwarded_group2++
~ article_sent = true


-> s3b_g2
* [Send it to your neighbours] 
~ group_3_opinion(theme, checked)
~ article_forwarded_group3++
~ article_sent = true


-> s3b_g3
* Don't send it to anyone{article_sent: else}. -> s3b_results

== s3b_g1 ==
Sent to Friends Group.-> s3b_choice
    
== s3b_g2 ==
Sent to Family Group. -> s3b_choice
{scold1_group2()}
== s3b_g3 == 
Sent to Neighbours Group.
-> s3b_choice
    
== s3b_results == 

Friends points: {group_1}
Family points: {group_2}
Neighbors points: {group_3}

-> scene_4_intro
