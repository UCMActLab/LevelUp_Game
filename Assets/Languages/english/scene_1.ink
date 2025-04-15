=== scene_1_intro ===
~news_count++

Here's article number 1. 

+ [Read the article] -> s1_article
+ [Skip the article] -> s1_choice

== s1_article ==
You read the article. 
    ~news_read++
    -> s1_choice


== s1_choice == 
What do you want to do? 

+ [Send it to Group 1] You send it to group 1.
    ~ group_1++
    -> s1_results
+ [Send it to Group 2] You send it to group 2.
    ~ group_2++ 
    -> s1_results
+ [Send it to Group 3] You send it to group 3.
    ~ group_3++
    -> s1_results
+ [Don't send it to anyone]
    -> s1_results

== s1_results == 

This is what will result from your choice. 
Number of news pieces received: {news_count}
Number of news pieces read: {news_read}

Group 1: {group_1}
Group 2: {group_2}
Group 3: {group_3}

-> scene_2_intro
