=== scene_5_intro ===
~news_count++
Here's article number 5. 

+ [Read the article] -> s5_article
+ [Skip the article] -> s5_choice

== s5_article ==
You read the article. 
    ~news_read++
    -> s5_choice


== s5_choice == 
What do you want to do? 

+ [Send it to Group 1] You send it to group 1.
    ~group_1++
    -> s5_results
+ [Send it to Group 2] You send it to group 2.
    ~group_2++ 
    -> s5_results
+ [Send it to Group 3] You send it to group 3.
    ~group_3++
    -> s5_results
+ [Don't send it to anyone]
    -> s5_results

== s5_results == 

This is what will result from your choice. 
Number of news pieces received: {news_count}
Number of news pieces read: {news_read}
Group 1: {group_1}
Group 2: {group_2}
Group 3: {group_3}

This is the end of this prototype. 

-> END
