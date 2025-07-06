// The variables and lists below are there to set up the database. May have to change some of these. 

TODO Add date to the list of data that players need to check 


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


// The function below is to print out the source of the article, to facilitate both the labels in the database and localization. 

=== function display_source()
{ 
    - comes_from == "blog":
        ~ return ("This is a blog post.")
    - comes_from == "newspaper":
        ~ return ("This is a newspaper article.")
  - else:
        ~ return ("This is a social media post.")
}

TODO THE FUNCTION BELOW IS JUST FOR THE PURPOSES OF THE PROTOTYPE, IN THE UNITY VERSION, THIS SHOULD PLUG IN TO THE EXTERNAL DATABASE

=== function article_data(print_article, what)  // this function is actually the database 
{print_article:
    - art1:
        ~ return data (what, "AFFECTED BY THE BLACKOUT? HERE’S HOW YOU CAN RECEIVE COMPENSATION", "social", "You may have been one of the many people who were affected by the loss of electricity for a whole day. If you suffered any kind of setback - such as having to discard food that could not be refrigerated, got stranded in a train station, or lost customers - and were born before 1975, you can apply for compensation funds to the Ministry of Energy. All you need to do is to sign up with your phone number on this link blackoutfunds.net and you will be sent a personalized link where you can submit your information and explain how you were affected by the blackout. You can receive up to 2000 euros, whatever your income is, within two weeks. Last month’s day-long blackout was yet another consequence the poor European energy policies. The war in Ukraine compromised our power sources across the union, and its members have to share power across countries in ways that are inadequate and unsustainable. Citizens should not pay the price of wars in countries outside the European Union and poor decision-making on the part of politicians.", scam, false)
    - art2:
        ~ return data (what, "USE OF MASKS RECOMMENDED AS NEW COVID VARIANT ON THE RISE", "newspaper", "The World Health Organization (WHO) has issued a new recommendation to wear filtration masks (FFP2) for people over the age of 60 and immunocompromised people, after the rise in cases of a new strain, NB 1.8.1. that originated in China back in March. This is a precautionary measure to protect vulnerable populations, as well as to slow the spread of the virus. The spread of the new COVID-19 variant has sped up and has already reached the United States through their national airports. The NB.1.8.1 variant is associated with symptoms similar to those seen in earlier strains of the virus, according to Dr. [NAME HERE], Head of the department of Immunology at [MAIN SCHOOL OF MEDICINE IN THE COUNTRY]. Commonly reported symptoms include respiratory issues such as cough and sore throat, as well as systemic effects like fever and fatigue, according to Dr. [X].", science, true)
    - art3:
        ~return data (what, "GOVERNMENT UNVEILS NEW MEASURES TO LEGALIZE UNREGISTERED IMMIGRANTS", "newspaper", "The government announced new measures to facilitate the registration and legalization of immigrants who already live in the country. The new policies will allow immigrants who entered the country illegally to become registered and have access to work permits. The aim of these measures is to curb unregistered employment, and prevent situations of abuse where illegal employees are coerced to work under threat of being deported. Newly legalized migrants will have access to public healthcare and national welfare services, while also contributing taxes with their labor. Immigrants can apply to regularize their situation as of today by submitting their applications to the Home Office either online or by mail. Local social security offices will also have a desk to help immigrants with their paperwork, to speed up the process of legalization.", news, true)
    - art4:
        ~return data (what, "IVERMECTIN, THE MIRACLE DRUG", "social", "You may want to get hold of a bottle of Ivermectin to protect yourself from new diseases, including new strains of COVID-19. And you can just get it from the pet store! Ivermectin has been widely used for decades to treat parasitic infections, such as river blindness and scabies. It works by paralyzing and killing parasites and has proven to be extremely effective and safe when used as directed for these approved purposes. It is cheap, and it is said that it can be effective against infections and diseases on people as well. ", science, false)
    - art5:
        ~return data (what, "CLIMATE CHANGE IS A LIE: ROMAN ROUND FOUND UNDER GLACIE", "blog", "The remains of a Roman road have been found at 3000 meters above sea level in Switzerland, after a glazier has melted. This demonstrates that there was no ice in the 1st century A.D., and that the Alps are the result of the Great Quaternary Glaciation until the small Ice Age (1950). In a new blow to the climate change alarmists, an expert has explained that during Roman times, they didn’t build a tunnel to cross the glacier, simply because there was no ice. It is impossible that we went through our hottest July in 120.000 years, and yet 2000 years ago the glacier that covered this road didn’t exist.", conspiracy, false)
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
    


