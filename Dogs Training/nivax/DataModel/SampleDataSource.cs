using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.ApplicationModel.Resources.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

// The data model defined by this file serves as a representative example of a strongly-typed
// model that supports notification when members are added, removed, or modified.  The property
// names chosen coincide with data bindings in the standard item templates.
//
// Applications may use this model as a starting point and build on it, or discard it entirely and
// replace it with something appropriate to their needs.

namespace PlanningDairyTemplate.Data
{
    /// <summary>
    /// Base class for <see cref="SampleDataItem"/> and <see cref="SampleDataGroup"/> that
    /// defines properties common to both.
    /// </summary>
    [Windows.Foundation.Metadata.WebHostHidden]
    public abstract class SampleDataCommon : PlanningDairyTemplate.Common.BindableBase
    {
        private static Uri _baseUri = new Uri("ms-appx:///");

        public SampleDataCommon(String uniqueId, String title, String subtitle, String imagePath, String description)
        {
            this._uniqueId = uniqueId;
            this._title = title;
            this._subtitle = subtitle;
            this._description = description;
            this._imagePath = imagePath;
        }

        private string _uniqueId = string.Empty;
        public string UniqueId
        {
            get { return this._uniqueId; }
            set { this.SetProperty(ref this._uniqueId, value); }
        }

        private string _title = string.Empty;
        public string Title
        {
            get { return this._title; }
            set { this.SetProperty(ref this._title, value); }
        }

        private string _subtitle = string.Empty;
        public string Subtitle
        {
            get { return this._subtitle; }
            set { this.SetProperty(ref this._subtitle, value); }
        }

        private string _description = string.Empty;
        public string Description
        {
            get { return this._description; }
            set { this.SetProperty(ref this._description, value); }
        }

        private ImageSource _image = null;
        private String _imagePath = null;
        public ImageSource Image
        {
            get
            {
                if (this._image == null && this._imagePath != null)
                {
                    this._image = new BitmapImage(new Uri(SampleDataCommon._baseUri, this._imagePath));
                }
                return this._image;
            }

            set
            {
                this._imagePath = null;
                this.SetProperty(ref this._image, value);
            }
        }

        public void SetImage(String path)
        {
            this._image = null;
            this._imagePath = path;
            this.OnPropertyChanged("Image");
        }
    }

    /// <summary>
    /// Generic item data model.
    /// </summary>
    public class SampleDataItem : SampleDataCommon
    {
        public SampleDataItem(String uniqueId, String title, String subtitle, String imagePath, String description, String content, SampleDataGroup group)
            : base(uniqueId, title, subtitle, imagePath, description)
        {
            this._content = content;
            this._group = group;
        }

        private string _content = string.Empty;
        public string Content
        {
            get { return this._content; }
            set { this.SetProperty(ref this._content, value); }
        }

        private SampleDataGroup _group;
        public SampleDataGroup Group
        {
            get { return this._group; }
            set { this.SetProperty(ref this._group, value); }
        }
        private string _createdon = string.Empty;
        public string CreatedOn
        {
            get { return this._createdon; }
            set { this.SetProperty(ref this._createdon, value); }
        }
        private string _createdtxt = string.Empty;
        public string CreatedTxt
        {
            get { return this._createdtxt; }
            set { this.SetProperty(ref this._createdtxt, value); }
        }

        private string _Colour = string.Empty;
        public string Colour
        {
            get { return this._Colour; }
            set { this.SetProperty(ref this._Colour, value); }
        }
        private string _bgColour = string.Empty;
        public string bgColour
        {
            get { return this._bgColour; }
            set { this.SetProperty(ref this._bgColour, value); }
        }
        private string _createdontwo = string.Empty;
        public string CreatedOnTwo
        {
            get { return this._createdontwo; }
            set { this.SetProperty(ref this._createdontwo, value); }
        }
        private string _createdtxttwo = string.Empty;
        public string CreatedTxtTwo
        {
            get { return this._createdtxttwo; }
            set { this.SetProperty(ref this._createdtxttwo, value); }
        }

        private string _currentStatus = string.Empty;
        public string CurrentStatus
        {
            get { return this._currentStatus; }
            set { this.SetProperty(ref this._currentStatus, value); }
        }
    }

    /// <summary>
    /// Generic group data model.
    /// </summary>
    public class SampleDataGroup : SampleDataCommon
    {
        public SampleDataGroup(String uniqueId, String title, String subtitle, String imagePath, String description)
            : base(uniqueId, title, subtitle, imagePath, description)
        {
        }

        private ObservableCollection<SampleDataItem> _items = new ObservableCollection<SampleDataItem>();
        public ObservableCollection<SampleDataItem> Items
        {
            get { return this._items; }
        }
        
        public IEnumerable<SampleDataItem> TopItems
        {
            // Provides a subset of the full items collection to bind to from a GroupedItemsPage
            // for two reasons: GridView will not virtualize large items collections, and it
            // improves the user experience when browsing through groups with large numbers of
            // items.
            //
            // A maximum of 12 items are displayed because it results in filled grid columns
            // whether there are 1, 2, 3, 4, or 6 rows displayed
            get { return this._items.Take(12); }
        }
    }

    /// <summary>
    /// Creates a collection of groups and items with hard-coded content.
    /// </summary>
    public sealed class SampleDataSource
    {
        private static SampleDataSource _sampleDataSource = new SampleDataSource();

        private ObservableCollection<SampleDataGroup> _allGroups = new ObservableCollection<SampleDataGroup>();
        public ObservableCollection<SampleDataGroup> AllGroups
        {
            get { return this._allGroups; }
        }

        public static IEnumerable<SampleDataGroup> GetGroups(string uniqueId)
        {
            if (!uniqueId.Equals("AllGroups")) throw new ArgumentException("Only 'AllGroups' is supported as a collection of groups");
            
            return _sampleDataSource.AllGroups;
        }

        public static SampleDataGroup GetGroup(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = _sampleDataSource.AllGroups.Where((group) => group.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public static SampleDataItem GetItem(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = _sampleDataSource.AllGroups.SelectMany(group => group.Items).Where((item) => item.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public SampleDataSource()
        {
           // String ITEM_CONTENT = String.Format("");

            var group1 = new SampleDataGroup("Group-1",
                    "Ways & Directions",
                    "Ways & Directions",
                    "Assets/Images/10.jpg",
                    "Dog training is the process of modifying the behavior of a dog, either for it to assist in specific activities or undertake particular tasks, or for it to participate effectively in contemporary domestic life. While training dogs for specific roles dates back to Roman times at least, the training of dogs to be compatible household pets developed with suburbanisation in the 1950s.");
            group1.Items.Add(new SampleDataItem("Group-1-Item-1",
                    "Body Language",
                    "Understanding Your Dog's Body Language. It can be strange to think about understanding your dog's body language. However, it is important to understand that your dog communicates with more than just his bark! Learning what it means when your dog does certain things can be helpful no matter what training technique you choose to employ.",
                    "Assets/DarkGray.png",
					"",            
                    "Details:\n\nUnderstanding Your Dog's Body Language. It can be strange to think about understanding your dog's body language. However, it is important to understand that your dog communicates with more than just his bark! Learning what it means when your dog does certain things can be helpful no matter what training technique you choose to employ. \n\nThe first thing you should realize is that many dogs speak with their eyes. You can communicate a lot with your dog just with your eyes! This includes facial expressions, and you'll be surprised as to how much information you can gain, and how much information you can transmit, just by using your eyes and making eye contact.\n\nDo keep in mind that in certain situations this type of eye contact may be read as being hostile. This can lead to aggressive behavior with your dog -- which leads to them having other problems. This is one great reason why it's important to study as much about your dog's body language (and how to use your own body language to communicate with your dog) as possible.\n\nRealize that when dogs are in wild packs these signals are used to communicate with one another. Dogs try to do this with you as well. It is our fault as humans that we often do not recognize this, and the signals go ignored. Examples of signals might include yawning, sniffing, freezing, licking, turning their head, and other signals.\n\nIf you choose to recognize what these different signals mean, your relationship with your dog will be greatly enhanced. You'll be able to communicate effectively and meet your dogs needs and wants. Your dog might become frustrated if you cannot understand what the signals mean; which can inhibit your training progress. No matter what type of training you plan on doing, you need to be sure you know as much as possible about your dog's body language.\n\nYou also need to pay attention to your own body language. Do not act timid or out of control in any way. Dogs can read this, and it may cause some to try and establish their role as the alpha. This can lead to clashes in interest, and can prevent you and your dog from having a close relationship.\n\nAs helpful as reading your dog's body language is, it is surprising how few dog owners actually know anything about it. By taking steps toward understanding body language, you're well on your way to effectively communicating with your dog, and developing a long-lasting relationship.",
                    group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Body Language", bgColour = "#6495ED", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/11.jpg")), CurrentStatus = "Dogs Training Guidance" });
            group1.Items.Add(new SampleDataItem("Group-1-Item-2",
                     "Anxiety",
                     "Dealing With Doggie Anxiety In addition to learning how to use many different commands, it's important to realize that your dog might become anxious in specific situations. This anxiety can impair the dog's ability to lead a happy life, and it can hinder training.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nDealing With Doggie Anxiety In addition to learning how to use many different commands, it's important to realize that your dog might become anxious in specific situations. This anxiety can impair the dog's ability to lead a happy life, and it can hinder training.\n\nSome dogs have separation anxiety. They do not like to be apart from their owner. When you are gone, they will participate in destructive behavior. Realize that dogs like to be with other dogs, or people, and do not like to be alone.\n\nOf course, it is not realistic that you are by your dog's side 24 hours a day. It is your goal to make your dog realize that even though you are leaving, you are coming back. Leave and come back very quickly to help your dog used to this. That means standing just outside the door and returning right away. Gradually, you can increase the length of time you are gone until your dog is no longer anxious.\n\nAnother tip is to not make a huge deal when you leave. If you take forever petting the dog and cooing over the dog, he will just be more upset when you go. He needs to realize that it is not a huge deal when you leave, because you will be returning. This can be difficult for you to do, especially if you miss your dog, but it is absolutely essential for both of your sake's.\n\nOther dogs might be anxious around certain other dogs or people. There is no simple method to deal with this, as the different circumstances require different methods. No matter what, it is your goal to make sure your dog feels safe and secure, and that you are by his side so there is nothing to fear.\n\nIf you are noticing severe anxiety problems, you will want to take your dog to his vet. They might be able to diagnose some underlying issue that you are not seeing. Otherwise, it is a matter of staying on top of it and helping your dog feel as safe and secure as possible. Focus on your exact issue and educate yourself enough to help your dog become less anxious.\n\nThis is yet another reminder that your dog is emotionally complex. It is not always easy for you as a dog owner, but it is absolutely necessary for the dog you love.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Anxiety", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/12.jpg")), CurrentStatus = "Dogs Training Guidance" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-3",
                     "Common Commands",
                     "Common Commands to Teach Your Dog Now comes the fun part! After you have worked on training methods, obedience training, and aggression control, it's time to train your dog with commands. When your dog is a puppy, he'll be ready to use some basic commands -- which is very exciting!",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nCommon Commands to Teach Your Dog Now comes the fun part! After you have worked on training methods, obedience training, and aggression control, it's time to train your dog with commands. When your dog is a puppy, he'll be ready to use some basic commands -- which is very exciting!\n\nCome is one of the most common commands. You'll want your dog to come when you call him for a variety of different reasons. There are many different things you can do to make this more effective. For example, give your dog a reason to come; such as receiving a tasty treat. When your dog does walk towards you, be sure to praise him soon so he knows he has done something good. This will mean he'll be more likely to repeat the action in the future.\n\nMany people also wish to teach the dog how to sit. Again, this is useful in a variety of situations, so it is essential to know. An easy way to do this is to ensure that you always say the words with the actual action. When your dog sits down, say the word sit. You can also hold a treat over your dog's head and move it back -- he will naturally sit down -- and give the sit command.\n\nIn addition to teaching your dog how to sit, you also want your dog to be able to stay. This is essential to help your dog avoid dangerous situations, or to simply obey your commands for a variety of different reasons.\n\nYou'll also want your dog to be able to lie down. This can be a difficult thing to train, so be sure you're using the best methods. You have to repeat this method over and over again so your dog is able to learn what to do each time.\n\nYet another common dog training technique is to heel. This is usually used to ensure that your dog walks right beside you. It is especially helpful for puppies whose instinct is to run freely. Eventually, your dog should be able to follow you no matter which direction you go in.\n\nClearly, there are several different commands you will want your dog to learn. It is your job as your dog's owner to teach them these things. It's not just for your benefit, there are benefits for him as well. It will help to ensure that your dog stays happy and realizes who is the boss.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Common Commands", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/13.jpg")), CurrentStatus = "Dogs Training Guidance" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-4",
                     "Advanced Commands",
                     "Advanced Commands to Teach Your Dog You've made it! Now that you have taught your dog some basic commands, and have dealt with some common dog issues, it's time to try the more advanced commands. It's important to understand that your dog may not be ready for more advanced commands.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nAdvanced Commands to Teach Your Dog You've made it! Now that you have taught your dog some basic commands, and have dealt with some common dog issues, it's time to try the more advanced commands. It's important to understand that your dog may not be ready for more advanced commands. Dogs don't always work on a schedule, so focus on your dog's individual needs before moving on.\n\nSome of the more advanced commands are just continuations of simpler commands. For example, you can have your dog sit and stay for longer periods of time now that they are ready. If you have been successfully using training techniques up to this point, this should not be an issue for you at all.\n\nYour dog can also be taught to seek, fetch, roll over, catch, shake, crawl, climb, jump, and many more. There is nothing more fun than having a dog who is well trained and can do all of these things! Not only will people be incredibly impressed when they notice how well behaved and smart your dog is, but it will help keep you and your dog happier as well. That's not even to mention the issue of safety. Being able to follow your commands will help to ensure that your dog stays as safe as possible.\n\nIf you have been successful with some of the earlier techniques, there is no reason why you can't be successful with these more advanced commands. Your dog is now used to being trained by you, so there should be no issues. Still, there are times when you are not sure what to do, or you reach a point where you're just a standstill. When this happens, it's time to further your education and watch demonstrations of other people training their dogs with advanced techniques.\n\nIn the past, you would have had to hire an expensive trainer to give your dog the best education possible. These days, there are several courses on the market that can turn you into an expert as well. Training your pet on your own can be a great challenge for you, and it can bring you closer to your pet than you have ever been before.\n\nFocus on advanced commands, and your dog will be more obedient than you ever thought possible. Your bond will deepen, and you'll feel incredibly connected with your pet. There are few things better than having a dog who adores you, and who is able to follow your directions, no matter how complex they get!",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Advanced Commands", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/14.jpg")), CurrentStatus = "Dogs Training Guidance" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-5",
                     "Aggression Problems",
                     "How to Deal With Dog Aggression Problems Up to this point, you have discovered some of the basics of dog training. However, it can be difficult to fathom focusing on training your dog to do tricks if your dog has aggression problems!",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nHow to Deal With Dog Aggression Problems Up to this point, you have discovered some of the basics of dog training. However, it can be difficult to fathom focusing on training your dog to do tricks if your dog has aggression problems! This is very unfortunate, but it's natural for some dogs. It's up to you to teach your dog that this aggression is not placed correctly in your home.\n\nThe first thing you need to do is understand why this aggression occurs. Firstly, this behavior can be common for certain types of dogs out in the wild. It can be difficult to get rid of these inborn habits. Another reason is because the dog might feel territorial. He wants to keep what is his -- his. Another issue might be that he is unfamiliar with his surroundings, and is skittish about them.\n\nYou may think you're in the clear if your dog doesn't have these symptoms, but it happens that some dogs develop aggression later on. Keep your eye out for behaviors that might be considered aggressive to see if they build. If your dog's demeanor changes, then aggression might develop.\n\nYour dog may feel like he rules the roost, so you need to ensure that the dog realizes you are in command, and you are the authority. It's important to never back down and never treat the dog like he is the ruler.\n\nOther dogs are beyond these beginning stages -- which can be quite serious. He might attack or hurt someone, so this needs to be dealt with right away. You can hire a dog trainer, or choose to get rid of the dog if this becomes an issue -- especially if you have young children in the house.\n\nAt the same time, it's important not to give up on your dog if you feel like there is hope. Keep on top of things and educate yourself as much as possible. Keep your dog fit and healthy, and consider how things are affecting your dog. Beyond that, examine specific requirements that your particular breed of dog has.\n\nJust be sure not to abuse your dog or try to fight back. This will just cause the problems to escalate, and is not healthy for you or the dog. It is all about finding effective methods, and maintaining your cool in establishing your role as the leader.\n\nHaving a dog with aggression problems, or something you think might become aggression, can be incredibly difficult. With some patience and some quality education, you're sure to beat this problem so that you have a loving dog in your household.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Aggression Problems", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/15.jpg")), CurrentStatus = "Dogs Training Guidance" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-6",
                     "Cratetraining",
                     "Effective Crate Training -- An Essential Step House training your dog is one of the most difficult things you can do. Now that you have chosen a training method, it's important to address this specific area of dog training as well.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nEffective Crate Training -- An Essential Step House training your dog is one of the most difficult things you can do. Now that you have chosen a training method, it's important to address this specific area of dog training as well. Taking advantage of crate training is essential, as it can be effective way to train.\n\nThere is a lot that goes into choosing a crate. You have to be mindful of the size of your dog, and the size the dog will grow into if it is a puppy. Also, choose from the different materials that are available so your dog is as comfy as possible.\n\nPut the crate somewhere where your dog can feel peaceful and happy. Make it very comfortable, so as not to intimidate the dog and make him feel anxious in his crate. You want only positive associations with the crate!\n\nThere are different situations where you will want to put your dog in his crate. If there is ever a time where you think your dog will get into something he is not supposed to, or you want to be sure he is extra safe, you can put him in the crate. Your dog will actually come to appreciate being in the crate, because he'll feel safe. You will appreciate the crate because you know your dog is safe, and that the items in your house are also safe!\n\nThe crate is very effective for house training (which is probably why you're interested in it!). The reason for this is because dogs do not like to go to the bathroom where they sleep. Since the crate will not allow them to leave, they will hold it until they are in a place with they are supposed to go to the bathroom.\n\nIt is important that you are very mindful of letting your dog use the bathroom often enough. This is especially true when they're the newest of puppies, as the bladder control is not the best!\n\nIf you want this method to be as effective as possible, you need to help your puppy develop good feelings about the crate. Toys, comfortable pillows, and food are positive items place in there. Your dog will learn that good things happen in the crate, and that is a safe and happy place to be.\n\nIf you have a puppy, it's an especially a good idea to start crate training as soon as possible. It will help you with your potty training efforts, and it will help to keep your dog safe and secure since you can't be there every second of the day. It will take some getting used to for your dog, but the efforts are well worth the positive results.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Cratetraining", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/16.jpg")), CurrentStatus = "Dogs Training Guidance" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-7",
                     "Dog Whispering",
                     "Dog Whispering -- Does it Work?Dog whispering is something that many people are interested in these days. It was brought into the limelight by Cesar Milan, who has the TV series The Dog Whisperer. People see how he immediately gets the dogs to adhere to his wishes, and it seems quite incredible!",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nDog Whispering -- Does it Work? Dog whispering is something that many people are interested in these days. It was brought into the limelight by Cesar Milan, who has the TV series The Dog Whisperer.People see how he immediately gets the dogs to adhere to his wishes, and it seems quite incredible! While Cesar seems to have a special talent, others can learn to be dog whisperers too -- even you! It is all about understanding the dog's body language and communicating with your own body language. Dogs are able to sense what should happen when you get a grasp on doing this.\n\nDogs are social creatures by nature, and are used to living in a pack in the wild. They follow one another's body language frequently, so it comes as no surprise that dog whispering is such a successful training technique.\n\nThe first thing you need to realize if you want to try dog whispering is that you are the leader. Dogs can feel like they are the Alpha, and that they are in command. This is wrong, and can lead to problems with aggression and disobedience. If you establish yourself as a leader, your dog will adore you and will adhere to your wishes.\n\nIt is best for you to educate yourself on the body language of dogs. For instance, how they turn their head, walk, and make other movements can give you clues about what your dog is thinking and going through. Also, learning what to do with your own body can be extremely helpful in day-to-day life. Even if you are going to be incorporating a different form of training, it is well worth your time to learn about dog whispering so the two methods can work together.\n\nTo see if this is right for you, you might choose to watch Cesar Milan's TV program. It is quite incredible the first time you see it, and you might pick up some helpful tips as well. There are also some books and courses on the market that can help you become a dog whisperer. While to many it might seem silly, it can be quite effective in helping you train your dog. It is all about creating a bond and understanding one another. All anyone wants in life is to be understood. This includes your dog!\n\nDog whispering is becoming quite popular, very quickly. The reasons are clear -- that's because it works. It's time for you to learn how to do this as well.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Dog Whispering", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/17.jpg")), CurrentStatus = "Dogs Training Guidance" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-8",
                     "Obedience Training",
                     "Essential Obedience Training -- What You Need to Know Obedience training can have different implications depending on the age of your dog. If you have a puppy, it's best to start training around the age of six weeks and up. Doing so beforehand will be too early, and waiting too long will delay some of the positive benefits you could be seeing.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nEssential Obedience Training -- What You Need to Know Obedience training can have different implications depending on the age of your dog. If you have a puppy, it's best to start training around the age of six weeks and up. Doing so beforehand will be too early, and waiting too long will delay some of the positive benefits you could be seeing. However, some dogs will not be ready until they are several months old, so you'll need to be mindful of what your dog is capable of.\n\nPuppies at this age are so incredibly young that some people worry that having formal sessions aren't good for them. The good news is that it is best to train them for just a few minutes a day, several different times a day. That way you can build up your obedience training over time as your dog becomes older and more receptive to training.\n\nDogs that are six months old and up are likely ready to be trained in a more formal manner. It is best to either hire a trainer, or take a dog training course to ensure that you have all of the latest methods at your disposal. You'll find that there are several effective courses on market that can help you even if you have never trained a dog before!\n\nThere are some people who have adopted an older dog, and aren't sure that dog is trainable. The good thing is that you can gradually make positive changes for the dog. The bad news is that it can be difficult if many of these behaviors and habits have been set for a long period of time. The important thing is that you allow the dog to get used to his new environment, and you should become as informed as possible about effective training techniques.\n\nClearly, obedience training is very important. You've already learned about the different types of training, as well as the usages of crate training. Now it's time to formalize your training plan by becoming knowledgeable and taking great strides to do what is right for your dog. It might seem like a long shot now, but the chances are good that you can have your dog trained perfectly, in a reasonable amount of time. Dogs truly are man's best friends, and there is absolutely no reason why you cannot successfully train your dog.\n\nHowever, the longer you wait to teach yourself what to do, the more difficult it will be for you. Take advantage of this opportunity to learn what you can about obedience training and put the techniques into practice as soon as possible.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Obedience Training", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/18.jpg")), CurrentStatus = "Dogs Training Guidance" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-9",
                     "Trainer",
                     "Train Your Dog Yourself, or Hire a Trainer? There are many people who have a new dog, and are at a loss as to what to do about training. Not training your dog can lead to a variety of problems, and to make life miserable for both of you.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nTrain Your Dog Yourself, or Hire a Trainer? There are many people who have a new dog, and are at a loss as to what to do about training. Not training your dog can lead to a variety of problems, and to make life miserable for both of you. Clearly, there is a need for training -- but should you hire a trainer?\n\nThere many positives to hiring a trainer. First of all, you'll have contact with someone who has experience and has successfully trained dogs before. This can cut down on the time it takes for your dog to be trained -- and they can cut down on the amount of work that you need to do. While you definitely need to be involved, the trainer will train you as well to make your life easier.\n\nUnfortunately, hiring a dog trainer can be very expensive. Rates vary widely depending on where you go to get the training -- but you can expect to pay fairly big money. If you're dealing with the expenses of your dog anyway, this extra burden could be too much for some families. It can pay off in the long run, but it leads people to wonder if they can really train their dog on their own.\n\nMany people do successfully train their dog on their own! Of course, it's not enough to just wing it -- you need effective strategies that are proven to work. The good news is that there are books, e-books, and courses out there that can help you. Authors and product owners have realized that there is a need in the market for people who want to train a dog on their own. Not only is it less expensive, but it is very fulfilling to realize that you have trained your dog.\n\nYou should strive to learn everything there is to know about raising a dog, and training a dog. Reading this information on your own can help you become a better dog owner. Also, training your dog can help you and your dog become closer than ever before. Dog really is man's best friend, and when you make such a huge effort for the dog, he will realize it and you'll have a happy, healthy relationship.\n\nThe decision whether to hire a trainer, or to train your dog on your own is not an easy one. Examine your family's finances, and the amount of time you have to train your dog. Many people find that they would choose one of the courses on the market to successfully train their dog as soon as possible, with the most effective methods possible.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Trainer", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/19.jpg")), CurrentStatus = "Dogs Training Guidance" });
			group1.Items.Add(new SampleDataItem("Group-1-Item-10",
                     "Types of Training",
                     "Types of Dog Training -- Which Method Will Be Effective For Your Dog? Since you're trying to train your dog, it's important to understand the different types of training that are available. Knowing this information will help you make an informed decision as to the type of training you want to use with your own pet.",
                     "Assets/DarkGray.png",
                     "",
                     "Details:\n\nTypes of Dog Training -- Which Method Will Be Effective For Your Dog? Since you're trying to train your dog, it's important to understand the different types of training that are available. Knowing this information will help you make an informed decision as to the type of training you want to use with your own pet. There are naturally ups and downs to each method, so this is an essential step.\n\nTraining with rewards is an incredibly popular method. Basically, whenever your dog does something correctly, you reward the him with a treat. You want him to recognize that when he does something good, he will get something good in return. It can be difficult to be consistent with the treats, so if you choose this method you need to be sure to stay consistent.\n\nChoke collar training is another method, though it is controversial. When the dog pulls away, the collar will have a choking effect. When they are walking correctly, it will be loose and comfortable. The thought is that will train your dog to behave correctly. Some people do not like this type of collar because it can result in injury.\n\nDog whispering is a training method that many people are intrigued by. It is all about communicating more effectively with your pet, and noticing his body language so you can determine what your dog needs and wants. This type of communication can be a highly successful training tool.\n\nClicker training is another method that is very effective, and it has its roots in psychology. Basically, the clicker will be paired with something, such as a treat. The dog will initially expect the treat. They get a treat and hear the clicker -- Eventually, the dog associates the clicker noise with doing something good, and the treat can be eliminated. \n\nWhistle training is also very popular. It is an ultrasonic whistle that a dog can hear, but humans cannot. This can be a little difficult to use, so it's important to be trained to use the whistle so it is as effective as possible.\n\nTraining your dog is undoubtedly a difficult task, and even a little bit intimidating if you are not sure how to go about it. Examine these different methods and decide what you think will work best for you. Then, it's very important to educate yourself as much as possible so you're able to start a training program and follow through with it through completion. This will lead to a happy, healthy life with your dog.",
                     group1) { CreatedOn = "Group", CreatedTxt = "Ways & Directions", CreatedOnTwo = "Item", CreatedTxtTwo = "Types of Training", bgColour = "#DAA520", Image = new BitmapImage(new Uri(new Uri("ms-appx:///"), "Assets/Images/20.jpg")), CurrentStatus = "Dogs Training Guidance" });
					 
            this.AllGroups.Add(group1);


			
			
         
        }
    }
}
