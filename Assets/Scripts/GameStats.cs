
/**
 *  Static GameStats class used to preserve data between scenes
 */
public static class GameStats
{

    private static int points;
    private static int days;
    private static int plants;
    private static int plantsLost;
    private static int bugs;


    public static int Points
    {
        get
        {
            return points;
        }
        set
        {
            points = value;
        }
    }

    public static int Days
    {
        get
        {
            return days;
        }
        set
        {
            days = value;
        }
    }

    public static int Plants
    {
        get
        {
            return plants;
        }
        set
        {
            plants = value;
        }
    }

    public static int PlantsLost
    {
        get
        {
            return plantsLost;
        }
        set
        {
            plantsLost = value;
        }
    }

    public static int Bugs
    {
        get
        {
            return bugs;
        }
        set
        {
            bugs = value;
        }
    }

    public static void AddPlant()
    {
        plants += 1;
    }

    public static void AddPlantLost()
    {
        plantsLost += 1;
    }

    public static void AddBug()
    {
        bugs += 1;
    }


    public static void Clear()
    {
        plants = 0;
        points = 0;
        plantsLost = 0;
        bugs = 0;
        days = 0;
    }


}
