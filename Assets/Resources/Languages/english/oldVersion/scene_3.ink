=== scene_3_intro ===
~ news_count++
~ article_sent = false

//~ temp print_article = LIST_RANDOM(LIST_ALL(articles)) // I'm hardcoding which article appears
~ temp print_article = art3
~ theme = article_data (print_article, Theme)
~ checked = article_data (print_article, verified)
Here's article number {news_count}. 
TODO: WHO IS SENDING THE ARTICLE?

Article headline: 
{article_data (print_article, headline)}

+ [Read the article] -> s3_article
+ [Skip the article] -> s3_choice

== s3_article ==
    ~ news_read++
{display_source()}
// The body of the text goes below.
{article_data (print_article, body)} -> s3_choice

== s3_choice == 
* [Send it to your friends.] 
~ group_1_opinion(theme, checked)
~ article_sent = true
-> s3_g1
    
* [Send it to your family.] 
~ group_2_opinion(theme, checked)
~ article_sent = true
-> s3_g2
    
* [Send it to your neighbours.] 
~ group_3_opinion(theme, checked)
~ article_sent = true
-> s3_g3
    
* Don't send it to anyone{article_sent: else}. -> s3_results

== s3_g1 ==
Manny (friends): We're getting invaded!
Mariah (friends): This is just going to bring in more foreigners.
-> s3_choice
    
== s3_g2 ==
Felicia (granddaughter): I'm glad you're reading news from the newspaper now. 
-> s3_choice

== s3_g3 == 
Omar (neighbour): That is so good to know, thank you. I'll pass it on to a couple of friends of mine.
Paula (neighbours): (This message has been deleted by the user.)
Emma: Hope this is for the best. 
-> s3_choice

== s3_results == 
Friends points: {group_1}
Family points: {group_2}
Neighbors points: {group_3}
-> scene_4_intro
