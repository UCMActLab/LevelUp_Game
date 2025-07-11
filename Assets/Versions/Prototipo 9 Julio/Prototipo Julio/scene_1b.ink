=== scene_1b_intro ===
~ news_count++
~ article_sent = false

~ temp print_article = LIST_RANDOM(LIST_ALL(articles)) // This lines selects an article at random
~ theme = article_data (print_article, Theme)
~ checked = article_data(print_article, verified)
Here's article number {news_count} 

TODO: WHO IS SENDING THE ARTICLE?

ARTICLE RECEIVED
Article headline: {article_data (print_article, headline)}

+ [Read the article] -> s1b_article
+ [Skip the article] -> s1b_choice

== s1b_article ==
~ news_read++
{display_source()}
// The body of the text goes below.
{article_data (print_article, body)} -> s1b_choice

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
~ group_1_opinion(theme, checked)
-> s1b_choice
    
== s1b_g2 ==
Sent to Family Group. 
~ group_2_opinion(theme, checked)
{scold1_group2()}
-> s1b_choice
== s1b_g3 == 
Sent to Neighbours Group.
~ group_3_opinion(theme, checked)
-> s1b_choice
    
== s1b_results == 

Friends points: {group_1}
Family points: {group_2}
Neighbors points: {group_3}

-> scene_2_intro
