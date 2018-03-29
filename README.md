# DoNet45CodeSnippet
使用C#开发中用到的一些简单代码示例片段
1.DoNetty【NIO（异步非阻塞）框架】简单使用Demo
2.Webp(Google定义的图片新格式，高质量压缩，压缩度高)，但是小号计算机性能较高具体可以在Demo中查看，压缩速度比一般压缩方法要慢很多
3.死锁研究：多线程死锁预防(死锁易出现于WPF/WINFORM/ASP.NET中，控制台中不易复现)：
  1）尽量都用 async await不要用.wait()  2）尽量在await 方法名.ConfigureAwait(false) 或者Task.Yield()
4.标准范式工具类中封装了一些简单调用可以作为参考
