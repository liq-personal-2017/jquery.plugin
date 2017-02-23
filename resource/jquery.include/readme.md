## jquery.include

首先说一下这个插件的作用：

这个插件是用来添加项目中用到的所有插件的 js 以及 css ，也就是说不再使用过多的 **script**|**link** 标签进行引用资源，并且能做到页面初始化逻辑将会放在插件资源加载完毕之后执行

一般我们的页面都是在dom加载完毕（window.onload）之后执行，在使用了jquery的情况下,用的最多的是如下两种形式：

    //1:
    $(function readycall(){
        //do sth...
    })
    //2:
    $(document).ready(function readycall(){
        //do sth...
    });
    

javascript 的加载会阻塞页面的继续渲染，所以一般我们建议把script放到页面的结尾处添加
但是这解决不了一个问题，页面上script标签太多，看着页面太乱。


使用这个插件之后，页面上将不会出现一大堆的各种 *script* 和 *link* 标签，变成了一个个有意义的语句。

比如我需要加载一个 *list* ，页面上的引入将会变成下面这种形式（除了 file 之外，其他所有的参数都是可选的）：


     $.include({
         cache: true,
         ismin: false,
         rootpath: '',
         file: ['list', 'h1.js']
     })

详细例子可以参考 */test/testinclude.html*，引入 **jquery.include** 之后，凡是以 *$(function readycall(){})* 或者 *$(document).ready(function readycall(){})* 形式添加的readycall，都会在所有的资源script加载之后执行

本插件 **支持 IE8** , IE7 及以下版本请自行测试。

注意，这个插件只是实现了 **异步加载script** 并且能够使添加到 *jquery* 上的 DOM 加载完毕回调的时间改到所有的 script 内容加载完成之后。

并不能解决依赖问题，即：如果同时加载了两个文件 A.JS 和 B.JS 并且 B 依赖于 A ，则可能导致出错（ B 先于 A 加载完成，并自动开始初始化，这时 A 还未加载完毕，导致 B 无法加载）。本插件并不是 requirejs ，也不以 requirejs 作为目标。

本插件能做到延迟 readylist 的执行是因为我修改了 jquery ，所以如果您打算使用其他版本的 jquery ，则需要修改您使用的 jquery 的源码，具体可以参照我提供的源码进行修改。

本插件做到了延迟 readlist 的执行，但是并不是真正意义上的延迟了 window onload 的触发（虽然我一开始的思路是从这里触发）。

为了兼容 IE8 ,插件内会有一些奇奇怪怪的代码

然后我的项目里头用了一个 localhostIp 的关键字，这个关键字是会被 output/ScriptCs.cs 类处理掉，所以如果不是 c# 项目，可以直接改成对应的 IP  地址即可



