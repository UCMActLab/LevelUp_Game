=== scene_3_intro ===
~ news_count++
~ article_sent = false

//~ temp print_article = LIST_RANDOM(LIST_ALL(articles)) // I'm hardcoding which article appears
~ temp print_article = HC_art3
~ theme = article_data_HC (print_article, Theme)
~ checked = article_data_HC (print_article, verified)
Here's article number {news_count}. 
TODO: WHO IS SENDING THE ARTICLE?

ARTICLE RECEIVED
Article headline: {article_data_HC (print_article, headline)}

+ [Read the article] -> s3_article
+ [Skip the article] -> s3_choice

== s3_article ==
    ~ news_read++
{display_source()}
// The body of the text goes below.
{article_data_HC (print_article, body)}-> s3_choice

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
~ article_sent = true

    -> s3_g3
    
* [Don't send it to anyone{article_sent: else}.] -> s3_results

== s3_g1 ==
Sent to Friends Group.
~ group_1_opinion(theme, checked)
Manny: We're getting invaded!
Mariah: This is just going to bring in more foreigners. -> s3_choice
    
== s3_g2 ==
Sent to Family Group.
~ group_2_opinion(theme, checked)
Felicia: I'm glad you're reading news from the newspaper now. 
-> s3_choice

== s3_g3 == 
Sent to Neighbours Group.
~ group_3_opinion(theme, checked)
 Omar: That is so good to know, thank you. I'll pass it on to a couple of friends of mine.
 Paula: (This message has been deleted by the user.)
 Emma: Hope this is for the best. 
 -> s3_choice

== s3_results == 

Friends points: {group_1}
Family points: {group_2}
Neighbors points: {group_3}


-> scene_3b_intro
