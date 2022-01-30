# GitBlogHelper

In Gitblog, especially Jekyll, Directory can be categories but it does not manage directory structers.

So it is hard to implement a hierarchical Category Sidebar. 

This Program helps to implement and manage that structures.

1. it update ```navigation.yml``` using directory infomations.

  + The infomations can be accessed using ```site.data.navigation[page.sidebar.nav]```.

  + ```sidebar.html``` or ```nav_list.html``` can use these informations.

2. It makes ```directory.md``` using a given md files. 
 
  + URLs end with Category Name open these md files. Just editing template md file is sufficient.

For detail., Read [my Blog Posts](https://mona04.github.io/posts/jekyll/Hierarchy-Category-Sidebar/)

## Setting

### Setting.xml

It has two values, ```PostFolder``` and ```GitBlogFoler```

```GitBlogFoler``` is Absolute Directory where your gitblog base forler exists.

```PostFolder``` is Absolute Directory where your posts exists. 

if you have more than two directory like ```[GitBlogFolder]/Category1/_posts/``` and ```[GitBlogFolder]/Category2/_posts/```, merge these into one foler like ```[GitBlogFolder]/Posts/Category1/_posts/``` 

### navigation.yml

It is almost same as ```_data/navigation.yml```. 

It read ```navigation.yml``` and append directory structures. 

Makes sure it have one more ```-title:``` and last one not have children of ```-titles:```.

### index.md

Each of Directory, it create ```[DirectoryName].md``` using ```index.md``` and replacing ```CATEGORY``` with each of Directory Names.

Default ```index.md```is sufficent to filter the just category.
