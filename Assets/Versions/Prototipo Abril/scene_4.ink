=== scene_4_intro ===
~ news_count++
~ temp print_article = LIST_RANDOM(LIST_ALL(articles))
~ theme = article_data (print_article, Theme)
~ checked = article_data (print_article, verified)
Here's article number {news_count}. 

You can read the headline: {article_data (print_article, headline)}
This topic of this piece is {theme} and verification is {checked}

+ [Read the article] -> s4_article
+ [Skip the article] -> s4_choice

== s4_article ==
    ~ news_read++
The source is {article_data(print_article, sources)}
The body of the text is {article_data (print_article, body)}-> s4_choice

== s4_choice == 
...
 What do you want to do? 

* [Send it to Group 1] You send it to group 1.
    ~ group_1_opinion(theme, checked)
    -> s4_g1
    
* [Send it to Group 2] You send it to group 2.
    ~ group_2_opinion(theme, checked)
    -> s4_g2
    
* [Send it to Group 3] You send it to group 3.
    ~ group_3_opinion(theme, checked)
    -> s4_g3
    
+ [Don't send it to anyone] -> s4_results

== s4_g1 ==
    This is the reaction of group 1 to your message.
    {scold2()}
    The value of group 1 is {group_1}-> s4_choice
    
== s4_g2 ==
    This is the reaction of group 2 to your message. 
    The value of group 2 is {group_2}-> s4_choice

== s4_g3 == 
    This is the reaction of group 3 to your message.
    {scold2()}
    The value of group 3 is {group_3}-> s4_choice
    


== s4_results == 

This is what will result from your choice. 

Number of news pieces received: {news_count}
Number of news pieces read: {news_read}

Group 1: {group_1}
Group 2: {group_2}
Group 3: {group_3}

-> checkin_2