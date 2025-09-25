// The variables and lists below are there to set up the database. May have to change some of these. 


LIST articles = art1, art2, art3, art4, art5, art6, art7, art8, art9, art10 //this list is the number of items in the database. New entry in the database means adding an item here. 

LIST Data = headline, sources, body, Theme, verified // these are the fields of each database item

LIST source = blog, newspaper, social // these are the list of labels to mark the source.

LIST themes = news, science, conspiracy, computers, scam // these are the labels to mark the topics that news can be about

~ temp print_article = LIST_RANDOM(LIST_ALL(articles))

// These are the functions that set up and contain the data

=== function data (what, headline_data, source_data, body_data, theme_data, verified_data) // this function sets up the database entry fields
{ what:
    - headline: ~ return headline_data
    - sources: ~ return source_data
    - body: ~ return body_data
    - Theme: ~ return theme_data
    - verified: ~ return verified_data
}

TODO THE FUNCTION BELOW IS JUST FOR THE PURPOSES OF THE PROTOTYPE, IN THE UNITY VERSION, THIS SHOULD PLUG IN TO THE EXTERNAL DATABASE

=== function article_data(print_article, what)  // this function is actually the database 
{print_article:
    - art1:
        ~ return data (what, "HEADLINE 1", "blog", "Paragraph 1", news, false)
    - art2:
        ~ return data (what, "HEADLINE 2", "newspaper", "Paragraph 2", science, false)
    - art3:
        ~return data (what, "HEADLINE 3", "social", "Paragraph 3", conspiracy, false)
    - art4:
        ~return data (what, "HEADLINE 4", "social", "Paragraph 4", computers, false)
    - art5:
        ~return data (what, "HEADLINE 5", "social", "Paragraph 5", scam, false)
    - art6:
        ~ return data (what, "HEADLINE 1", "blog", "Paragraph 1", news, true)
    - art7:
        ~ return data (what, "HEADLINE 2", "newspaper", "Paragraph 2", science, true)
    - art8:
        ~return data (what, "HEADLINE 3", "social", "Paragraph 3", conspiracy, true)
    - art9:
        ~return data (what, "HEADLINE 4", "social", "Paragraph 4", computers, true)
    - art10:
        ~return data (what, "HEADLINE 5", "social", "Paragraph 5", scam, true)
    
    - else: [Cannot find {print_article}]

    }
    


