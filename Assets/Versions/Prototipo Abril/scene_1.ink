=== scene_1_intro ===
....
Okay, let's do this for real

~ news_count++
TODO The line is the line that selects the article from the database. This should hook into the external database. 
~ temp print_article = LIST_RANDOM(LIST_ALL(articles)) 
~ theme = article_data (print_article, Theme)
~ checked = article_data (print_article, verified)

Here's article number {news_count}. 
You can read the headline: {article_data (print_article, headline)}

TODO: These values need to be taken from the database - this will be tags for each piece of news. The database also needs at least a headline and a text body. This will be the same in each scene. 

This topic of this piece is {theme} and verification is {checked}

+ [Read the article] -> s1_article
+ [Skip the article] -> s1_choice

=== s1_article ===
    ~ news_read++
Let's read the story. 
The source is {article_data (print_article, sources)}.
The body of the text is {article_data (print_article, body)} .-> s1_choice

== s1_choice == 
....
 What do you want to do? 

TODO:  This system is buggy - once all the options are exhausted, the program should move to the next knot automatically, without showing the option. I'm leaving it for now because I know that unity can be picky with how it displays ink text, and we may be able to fix this with conditional text in the choices. 

* [Send it to Group 1] You send it to group 1.
    ~ group_1_opinion(theme, checked)
    -> s1_g1

* [Send it to Group 2] You send it to group 2.
    ~ group_2_opinion(theme, checked)
    -> s1_g2
    
* [Send it to Group 3] You send it to group 3.
    ~ group_3_opinion(theme, checked)
    -> s1_g3
+ [Don't send it to anyone else.] -> s1_results
  
== s1_g1 ==
    This is the reaction of group 1 to your message.

    The value of group 1 is {group_1}-> s1_choice
    
== s1_g2 ==
    This is the reaction of group 2 to your message. 
    {scold1()}
    The value of group 2 is {group_2}-> s1_choice

== s1_g3 == 
    This is the reaction of group 3 to your message. 
    {scold2()}
    The value of group 3 is {group_3}-> s1_choice

== s1_results == 

This is what will result from your choice. 
Number of news pieces received: {news_count}
Number of news pieces read: {news_read}

Group 1: {group_1}
Group 2: {group_2}
Group 3: {group_3}

-> scene_2_intro
