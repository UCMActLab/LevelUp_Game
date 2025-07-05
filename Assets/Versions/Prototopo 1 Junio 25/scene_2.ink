=== scene_2_intro ===
~ news_count++
~ article_sent = false

//~ temp print_article = LIST_RANDOM(LIST_ALL(articles)) // I'm hardcoding which article appears
~ temp print_article = art2
~ theme = article_data (print_article, Theme)
~ checked = article_data(print_article, verified)
Here's article number {news_count} 

TODO: WHO IS SENDING THE ARTICLE?

Article headline: 
{article_data (print_article, headline)}

+ [Read the article] -> s2_article
+ [Skip the article] -> s2_choice

== s2_article ==
~ news_read++
{display_source()}
// The body of the text goes below.
{article_data (print_article, body)} -> s2_choice

== s2_choice == 
* [Send it to your friends.] 
~ group_1_opinion(theme, checked)
~ article_sent = true
-> s2_g1

* [Send it to your family] 
~ group_2_opinion(theme, checked)
~ article_sent = true
-> s2_g2

* [Send it to your neighbours] 
~ group_3_opinion(theme, checked)
~ article_sent = true
-> s2_g3

* Don't send it to anyone{article_sent: else}. -> s2_results

== s2_g1 ==
Mark (friends): Can you believe it? They just want to scare us again.
Mariah (friends): I don't really trust these WHO people.
{~Mariah|Mark|Manny}{scold1()}
-> s2_choice
    
== s2_g2 ==
Alfie (grandson): Take this seriously. The WHO was first organization to alert of the pandemic in 2020. 
Fred (son): How concerning. Please wear a mask.
{group2_names}{scold2()}
-> s2_choice

== s2_g3 == 
Paula (neighbours): Again? It never ends, does it?
Omar (neighbours): I have some masks somewhere. Do you need some?
-> s2_choice
    
== s2_results == 
Friends points: {group_1}
Family points: {group_2}
Neighbors points: {group_3}
-> checkin_1
