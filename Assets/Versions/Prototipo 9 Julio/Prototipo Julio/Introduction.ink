// This is an example of how the game would start 

== tutorial_check == 
Would you like to read the tutorial? 
* [Yes.] -> Introduction
* [No.] -> scene_0_intro

== Introduction ==

In this game, you will interact with different groups, who will send you news articles, blog and social media posts. You can interact with them by forwarding them news (or not) and see how they react to what you send them.
Each group will react to what you send them differently - some will be interested, some will not like the news you send to them. Pay attention to their reactions to the news. Can you guess why they react to the news the way they do? Can you figure out where their reaction comes from? 

Would you like to learn more about these groups?
* [Yes.]-> explanation_groups
* [No, I already know.] -> article_tutorial

== explanation_groups ==

* [Friends] -> explain_g1
* [Family] -> explain_g2
* [Neighbours] -> explain_g3
* -> article_tutorial // this is not an option, it just means that once all the groups have been described the game will move to the next knot. 

== explain_g1 ==
Your friends are people whom you see in your social circles - the regulars at the coffee shop, the bar, the hair salon or the barber. You know each other of many years - some of them have been your coworkers, others are part of your sports fan group. They are a bit jaded and tired. 
/*They believe news about disinformation. They do not like science. They believe conspiracy theories. They do not like computers and artificial intelligence . 
They often fall from scams*/
-> explanation_groups
== explain_g2 ==
Your family are your children and grandchildren. Your grandchildren in particular have taught you how to use your phone and messaging, so they are good at technology, and they seem to be up to date with the news. 
/* This is group 2
They are very critical of things that sound like disinformation. They believe in science. They get angry at conspiracy theories. They are very skeptical about artificial intelligence. They dislike anything that sounds like a scams.*/-> explanation_groups
== explain_g3 ==
Your neighbours are people that you have known for decades. Your children grew up together. You've spend many years taking care of each other, some of them are good friends, and others you find irritating, but you have learned to live together. 
/*This is group 3. They often fall for disinformation. They like science, but it is scary. They believe conspiracy theories. They think artificial intelligence is scary. They can fall from scams. */
-> explanation_groups


== article_tutorial ==
Now let's learn how to interact with these groups. Here's what you'll see when you receive a news piece. You will see the headline first. 

THE EUROPEAN UNION DEMANDS THAT COUNTRIES CUT DOWN PENSIONS 30 TO 40%

+ [Read the article] -> article
+ [Skip the article] -> opinion

== article ==
The source is social media.

For those of us who receive a pension, after many years of work and contributions to social security for many years. This sunken economy and this disastrous goverment that people chose, has caused the EU to require specific economic conditions and guarantees in order to receive rescue funds and loans. Among other measures, they have offered to cut down salaries of public servants - except the themselves, that is - and especially retired people. Rumor has it - so you can all get ready - that the cuts will be 30 or 40%, as they did in Greece. We can't allow this, and we must stop this before it's too late. We need to stop this. Please pass this on to your contacts, whether they are retired or not.
-> opinion

== opinion ==
...
So what do you think? 

+ [I think this is garbage.] -> explanation
+ [This is very interesting.] -> explanation
+ [I don't know what to think about this.]-> explanation

== explanation ==
{article: Good on you for reading the article!}{not article:How can you have an opinion if you didn't read the article?}
This is a piece of fake news - it does not have names of specific people, institutions, it doesn not include sources, and it is written in an incendiary way to get people riled up. This is not a fact-checked piece of news and it's not written professionally (although some fact-checked news can also be written poorly on occasion). The goal of this text is to outrage the reader, and to spread out the anger among other people. 

Many of these news are spread through social media and blogs - so remember to pay attention to the source of the articles you receive too. 

You decide whether you want to resend the news or not - if you do, observe how they react to each article. Some people just want to receive news that confirm how they feel - whether they are fact-checked or not. Some people may realize that the article is really a scam; others may not and fall for it. 

What you do with each article is up to you - whether to read it or not, or whether you send it to any of the groups. Sharing news is a way to reinforce social ties - or alienate others. 

-> scene_0_intro

