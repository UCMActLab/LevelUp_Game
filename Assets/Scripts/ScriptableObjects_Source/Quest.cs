using UnityEngine;

public class Quest : ScriptableObject
{
    // TO DO
    public struct ToDo
    {
        public int falseArticlesToSkip;

        public int articlesToIdentify; // identify/share

        public int toShareWithFriends;
        public int toShareWithNeighbours;
        public int toShareWithFamily;

        public int toRead; // articles to read
    }

    public int totalArticles;
    public bool groupsHaveThemes;

    public int GetMaxPossibleScore() { return this.toDo.articlesToIdentify + this.toDo.toShareWithNeighbours + this.toDo.toShareWithFriends + this.toDo.toShareWithFamily; }

    public void BuildQuest(int numTrueArticles, int totalArticles, int numGroups, int articlesToRead, bool groupsHaveThemes)
    {
        this.totalArticles = totalArticles;

        toDo.articlesToIdentify = numTrueArticles;
        toDo.falseArticlesToSkip = this.totalArticles - numTrueArticles;

        if (numGroups > 0)
        {
            for (int i = 0; i < toDo.articlesToIdentify; ++i)
            {
                int rand = Random.Range(0, numGroups + 1);
                if (rand == 0) toDo.toShareWithFamily++;
                if (rand == 1) toDo.toShareWithFriends++;
                if (rand == 2) toDo.toShareWithNeighbours++;
            }
        }

        this.groupsHaveThemes = groupsHaveThemes;

        toDo.toRead = articlesToRead;
    }

    public void EvaluateArticle(ArticleGameObject data)
    {
        if (data.HasSharedArticle)
        {
            // add "with wich group have we shared?"

            Debug.LogError("TE QUEDASTE PENSANDO EN CÓMO PUNTUAR LAS TEMÁTICAS");

            if(data.IsTrue)
            {
                done.identifiedArticles++;

                for (int i = 0; i < data.HasSharedWithGroups.Length; ++i)
                {
                    if (data.HasSharedWithGroups[i])
                    {
                        if (groupsHaveThemes)
                        {
                            if (TopicsDictionary.topics[data.Data.theme] == 
                                LevelManager.Instance.GetGroupTheme(i))
                            {
                                done.themesCorrectlyAddressed++;
                            }
                        }
                        else
                        {
                            if (i == 0) done.sharedWithFamily++;
                            else if (i == 1) done.sharedWithFriends++;
                            else if (i == 2) done.sharedWithNeighbours++;
                        }
                    }

                }
            }
            else
            {
                done.falseArticlesShared++;
            }

            Debug.LogError("TODO: SharingPoints");
            
            //                  TODO
            // ============================================

            // sharedWithFriends
            // sharedWithNeighbours
            // sharedWithFamily

            // ============================================
        }

        if (data.HasReadArticle) done.readedArticles++;
    }

    // DONE 
    public struct Done
    {
        public int identifiedArticles; // positive
        public int falseArticlesSkipped; // positive
        public int sharedWithFriends; // positive
        public int sharedWithNeighbours; // positive
        public int sharedWithFamily; // positive

        public int themesCorrectlyAddressed; // positive

        public int falseArticlesShared; // negative

        public int readedArticles; // neutral -> doesn't awards points
    }

    public ToDo toDo;
    public Done done;
}
