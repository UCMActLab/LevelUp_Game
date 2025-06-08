=== scene_4_intro ===
~ news_count++
~ article_sent = false

//~ temp print_article = LIST_RANDOM(LIST_ALL(articles)) // I'm hardcoding which article appears
~ temp print_article = art4
~ theme = article_data (print_article, Theme)
~ checked = article_data (print_article, verified)
Here's article number {news_count}. 
TODO: WHO IS SENDING THE ARTICLE?

Article headline: 
{article_data (print_article, headline)}

+ [Read the article] -> s4_article
+ [Skip the article] -> s4_choice

== s4_article ==
    ~ news_read++
{display_source()}
// The body of the text goes below.
{article_data (print_article, body)} -> s4_choice

== s4_choice == 
* [Send it to your friends.] 
~ group_1_opinion(theme, checked)
~ article_sent = true
-> s4_g1
    
* [Send it to your family.] 
~ group_2_opinion(theme, checked)
~ article_sent = true
 -> s4_g2
 
* [Send it to your neighbours.] 
~ group_3_opinion(theme, checked)
~ article_sent = true
  -> s4_g3
  
* Don't send it to anyone{article_sent: else}. -> s4_results

== s4_g1 ==
Manny (friends): I've heard this before. It's a good thing to keep handy.
Mark (friends): I didn't know it was so easy to get. I'm going to order some right now{s2_g1: and see if I can get rid of this virus once and for all}. 
-> s4_choice

== s4_g2 ==
Felicia (granddaughter): This is not true! Please don't send this to anyone, someone can get hurt. 
Fred (son): Don't take Ivermectin, it is dangerous. 
-> s4_choice

== s4_g3 == 
Paula (neighbours): Do you know if they have this at the pharmacy?
Emma (neighbours): No, this is for animals. You should not take it. 
{~Paula|Omar|Emma}{scold2()}
-> s4_choice

== s4_results == 
Friends points: {group_1}
Family points: {group_2}
Neighbors points: {group_3}
-> checkin_2