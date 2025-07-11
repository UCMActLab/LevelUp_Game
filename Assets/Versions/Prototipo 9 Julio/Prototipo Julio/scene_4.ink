=== scene_4_intro ===
~ news_count++
~ article_sent = false

//~ temp print_article = LIST_RANDOM(LIST_ALL(articles)) // I'm hardcoding which article appears
~ temp print_article = HC_art4
~ theme = article_data_HC (print_article, Theme)
~ checked = article_data_HC (print_article, verified)
Here's article number {news_count}. 
TODO: WHO IS SENDING THE ARTICLE?

ARTICLE RECEIVED
Article headline: {article_data_HC (print_article, headline)}

+ [Read the article] -> s4_article
+ [Skip the article] -> s4_choice

== s4_article ==
    ~ news_read++
{display_source()}
// The body of the text goes below.
{article_data_HC (print_article, body)}-> s4_choice

== s4_choice == 
* [Send it to your friends.] 
~ article_forwarded_group1++
~ article_sent = true

-> s4_g1
    
* [Send it to your family.] 
~ article_forwarded_group2++
~ article_sent = true

 -> s4_g2
* [Send it to your neighbours.] 
~ article_forwarded_group3++
~ article_sent = true

  -> s4_g3
* [Don't send it to anyone{article_sent: else}.] -> s4_results

== s4_g1 ==
Sent to Friends Group.
~ group_1_opinion(theme, checked)
Manny: I've heard this before. It's a good thing to keep handy.
Mark: I didn't know it was so easy to get. I'm going to order some right now{s2_g1: and see if I can get rid of this virus once and for all}. 
-> s4_choice

== s4_g2 ==
Sent to Family Group.
~ group_2_opinion(theme, checked)
Felicia: This is not true! Please don't send this to anyone, someone can get hurt. 
Fred: Don't take Ivermectin, it is dangerous. 
{scold1_group2()}
-> s4_choice

== s4_g3 == 
Sent to Neighbours Group.
~ group_3_opinion(theme, checked)
Paula (neighbours): Do you know if they have this at the pharmacy?
Emma (neighbours): No, this is for animals. You should not take it. 
-> s4_choice



== s4_results == 
Friends points: {group_1}
Family points: {group_2}
Neighbors points: {group_3}

-> checkin_2