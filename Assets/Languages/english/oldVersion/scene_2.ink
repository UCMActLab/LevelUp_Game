=== scene_2_intro ===
~news_count++

Here's article number 2. 

+ [Read the article] -> s2_article
+ [Skip the article] -> s2_choice

== s2_article ==
You read the article. 
    ~news_read++
    -> s2_choice


== s2_choice == 
What do you want to do? 

+ [Send it to Group 1] You send it to group 1.
    ~group_1++
    -> s2_results
+ [Send it to Group 2] You send it to group 2.
    ~group_2++ 
    -> s2_results
+ [Send it to Group 3] You send it to group 3.
    ~group_3++
    -> s2_results
+ [Don't send it to anyone]
    -> s2_results

== s2_results == 

This is what will result from your choice. 
Number of news pieces received: {news_count}
Number of news pieces read: {news_read}
Group 1: {group_1}
Group 2: {group_2}
Group 3: {group_3}

-> scene_3_intro
