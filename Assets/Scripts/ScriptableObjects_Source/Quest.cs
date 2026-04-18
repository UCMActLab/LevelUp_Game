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

    /// <summary>
    /// Evalúa la puntuación de un artículo para una quest.
    /// Si se ha evaluado como erróneo, devuelve False
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public bool EvaluateArticle(ArticleGameObject data)
    {
        bool correctlyIdentified = true;
        if (data.HasSharedArticle)
        {
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
                                LevelManager.Instance.GetGroupTheme(i + 1)) // i == 0 es el MainChat (donde se ven los artículos)
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
                correctlyIdentified = false;
            }

            Debug.LogError("TODO: SharingPoints");
        }
        else if (data.IsTrue)
        {
            correctlyIdentified = false;
        }

        if (data.HasReadArticle) done.readedArticles++;
        return correctlyIdentified;
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
