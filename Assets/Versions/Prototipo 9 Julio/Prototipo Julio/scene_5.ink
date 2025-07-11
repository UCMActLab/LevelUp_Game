=== scene_5_intro ===
~ news_count++
~ article_sent = false

//~ temp print_article = LIST_RANDOM(LIST_ALL(articles)) // I'm hardcoding which article appears
~ temp print_article = HC_art5
~ theme = article_data_HC (print_article, Theme)
~ checked = article_data_HC (print_article, verified)
Here's article number {news_count}. 
TODO: WHO IS SENDING THE ARTICLE?

ARTICLE RECEIVED
Article headline: {article_data_HC (print_article, headline)}

+ [Read the article] -> s5_article
+ [Skip the article] -> s5_choice

== s5_article ==
~ news_read++
{display_source()}
// The body of the text goes below.
{article_data_HC (print_article, body)}-> s5_choice

== s5_choice == 

* [Send it to your friends.] 
~ article_forwarded_group1++
~ article_sent = true

  -> s5_g1
* [Send it to your family] 
~ article_forwarded_group2++
~ article_sent = true

   -> s5_g2
* [Send it to your neighbours] 
~ article_forwarded_group3++
~ article_sent = true

   -> s5_g3
* [Don't send it to anyone{article_sent: else}.] -> s5_results

== s5_g1 ==
Sent to Friends Group.
~ group_1_opinion (theme, checked)
  Mariah: See? I knew it did not make any sense!
  Manny: I keep saying this, global warming is an old wives' tale.
  -> s5_choice
    
== s5_g2 ==
Sent to Family Group.
~ group_2_opinion (theme, checked)
Fred: Do you really believe this? This sounds wrong. 
Alfie: This is not true. They found a stretch of compacted land that may be as old as the Romans, but not a stone road. This is misinformation. 
Felicia: Don't believe what is in social media, please. 
-> s5_choice

== s5_g3 == 
Sent to Neighbours Group.
~ group_3_opinion (theme, checked)
Paula: I don't know what to think about this. 
Emma: How interesting! 
-> s5_choice

== s5_results == 
Friends points: {group_1}
Family points: {group_2}
Neighbors points: {group_3}


-> game_ending
