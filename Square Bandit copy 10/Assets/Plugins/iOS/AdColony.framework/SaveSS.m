//
//  SaveSS.m
//  SaveToAlbumTest
//
//  Created by Rheo Violenes on 11/02/15.
//  Copyright (c) 2015 f4f. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "SaveSS.h"
#import <UIKit/UIKit.h>
#import <MobileCoreServices/MobileCoreServices.h>
@import Social;
@import StoreKit;

static SaveSS *_saveSSInstance;


void _SaveToAlbum()
{
    //SaveSS *ss = [[SaveSS alloc]init];
    [_saveSSInstance SaveToAlbum];
}

void _ChoosePhoto()
{
    // SaveSS *cp = [[SaveSS alloc]init];
    [_saveSSInstance ChoosePhoto];
}

void _ShareToFacebook(const char* msg)
{
    //SaveSS *sf = [[SaveSS alloc]init];
    [_saveSSInstance ShareToFacebook:msg];
}

void _ShareToTwitter(const char* msg)
{
    //SaveSS *st = [[SaveSS alloc]init];
    [_saveSSInstance ShareToTwitter:msg];
}

void _ShareToFacebookCard(const char* msg)
{
    [_saveSSInstance ShareToFacebookCard:msg];
}

void _ShareToTwitterCard(const char* msg)
{
    [_saveSSInstance ShareToTwitterCard:msg];
}


void _ShowAlert(const char* msg)
{
    //SaveSS *sa = [[SaveSS alloc]init];
    [_saveSSInstance ShowAlert:msg];
}

void _RateUs(const char* _id)
{
    //SaveSS *ru = [[SaveSS alloc] init];
    [_saveSSInstance RateUs:_id];
}

void _MoreGames()
{
    [_saveSSInstance MoreGames];
}

void _DownloadGame(const char* gameId)
{
    [_saveSSInstance DownloadGame:gameId];
    
}

void _VisitWebpage(const char* address)
{
    [_saveSSInstance VisitWebpage:address];
    
}

extern UIViewController* UnityGetGLViewController();

@implementation SaveSS

+ (void)load
{
    [[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(CreatePlugin:) name:UIApplicationDidFinishLaunchingNotification object:nil];
}

+(void)CreatePlugin:(NSNotification *)notification
{
    _saveSSInstance = [[SaveSS alloc] init];
}




-(void)SaveToAlbum
{
    NSLog(@"Saving image");
    //
    //    NSArray *paths = NSSearchPathForDirectoriesInDomains(NSDocumentDirectory, NSUserDomainMask, YES);
    //    NSString *documentsDirectory = [paths objectAtIndex:0];
    //    documentsDirectory = [documentsDirectory stringByAppendingString:@"/ScreenShot15.png"];
    //
    //    NSLog(documentsDirectory);
    //    UIImage *imageToSave = [UIImage imageWithContentsOfFile:documentsDirectory];
    
    NSUserDefaults *defaults = [NSUserDefaults standardUserDefaults];
    NSString *path = [defaults objectForKey:@"shotToSave"];
    UIImage *imageToSave = [UIImage imageWithContentsOfFile:path];
    
    
    
    UIImageWriteToSavedPhotosAlbum(imageToSave, nil, nil, nil);
    
    NSLog(@"-----***** Hopefully Saved *******--------");
    imageToSave = nil;
}

-(void)ChoosePhoto
{
    //UIViewController *rootView = [UIApplication sharedApplication].keyWindow.rootViewController;
    UIViewController *rootView = UnityGetGLViewController();
    NSLog(@"Got root controller");
    
    if([self startMediaBrowserFromViewController:rootView usingDelegate:self] == NO)
    {
        //UnitySendMessage("LevelManager", "DisplayPhoto", "Failed");
        UIAlertView *alertView = [[UIAlertView alloc]initWithTitle:
                                  @"Photos are currently unavailable" message:nil delegate:
                                  self cancelButtonTitle:@"Ok" otherButtonTitles: nil];
        [alertView show];
    }
}

- (BOOL) startMediaBrowserFromViewController: (UIViewController*) controller
                               usingDelegate: (id <UIImagePickerControllerDelegate,
                                               UINavigationControllerDelegate>) delegate {
    NSLog(@"starting browser");
    
    if (([UIImagePickerController isSourceTypeAvailable:
          UIImagePickerControllerSourceTypeSavedPhotosAlbum] == NO)
        || (delegate == nil)
        || (controller == nil))
        return NO;
    
    UIImagePickerController *mediaUI = [[UIImagePickerController alloc] init];
    mediaUI.sourceType = UIImagePickerControllerSourceTypePhotoLibrary;
    
    //Allowing mediaUI.mediatype to default to Images
    
    // Displays saved pictures and movies, if both are available, from the
    // Camera Roll album.
    //    mediaUI.mediaTypes =
    //    [UIImagePickerController availableMediaTypesForSourceType:
    //     UIImagePickerControllerSourceTypeSavedPhotosAlbum];
    //    if(mediaUI.mediaTypes == kUTTypeMovie) return NO;
    
    // Hides the controls for moving & scaling pictures, or for
    // trimming movies. To instead show the controls, use YES.
    mediaUI.allowsEditing = NO;
    
    mediaUI.delegate = delegate;
    
    //UIView *topView = [[[[UIApplication sharedApplication] keyWindow] subviews] lastObject];
    
    //have to implement ipad specific code path
    if ([[UIDevice currentDevice].model hasPrefix:@"iPad"])
    {
        UIPopoverController *popoverController = [[UIPopoverController alloc] initWithContentViewController: mediaUI];
        [popoverController presentPopoverFromRect:CGRectZero inView:UnityGetGLViewController().view permittedArrowDirections:UIPopoverArrowDirectionAny animated:YES];
    }
    else [UnityGetGLViewController() presentModalViewController: mediaUI animated: YES];
    return YES;
    
    NSLog(@"browser started");
}

- (void)imagePickerController:(UIImagePickerController *)picker
didFinishPickingMediaWithInfo:(NSDictionary *)info
{
    //    NSLog(@"called back");
    //    if (picker.sourceType == UIImagePickerControllerSourceTypeCamera)
    //    {
    //        NSString *mediaType = [info objectForKey: UIImagePickerControllerMediaType];
    //        UIImage *originalImage, *editedImage, *imageToUse;
    //
    //        // Handle a still image picked from a photo album
    //        if (CFStringCompare ((CFStringRef) mediaType, kUTTypeImage, 0)
    //            == kCFCompareEqualTo)
    //        {
    //
    //            editedImage = (UIImage *) [info objectForKey:
    //                                       UIImagePickerControllerEditedImage];
    //            originalImage = (UIImage *) [info objectForKey:
    //                                         UIImagePickerControllerOriginalImage];
    //
    //            if (editedImage) {
    //                imageToUse = editedImage;
    //            } else {
    //                imageToUse = originalImage;
    //            }
    //            // Do something with imageToUse
    //            UIImageWriteToSavedPhotosAlbum(imageToUse, nil, nil, nil);
    //            //Attempt to forcibly eject memory hogs
    //            originalImage = nil;
    //            editedImage = nil;
    //            imageToUse = nil;
    //
    //
    //            //UnitySendMessage("selfieManager","ReceivePhotoPath",[filePath UTF8String]);
    //             NSLog(@"saved");
    //        }
    //    }
    
    // else
    // {
    NSString *mediaType = [info objectForKey: UIImagePickerControllerMediaType];
    UIImage *originalImage, *editedImage, *imageToUse;
    
    // Handle a still image picked from a photo album
    if (CFStringCompare ((CFStringRef) mediaType, kUTTypeImage, 0)
        == kCFCompareEqualTo)
    {
        
        editedImage = (UIImage *) [info objectForKey:
                                   UIImagePickerControllerEditedImage];
        originalImage = (UIImage *) [info objectForKey:
                                     UIImagePickerControllerOriginalImage];
        
        if (editedImage) {
            imageToUse = editedImage;
        } else {
            imageToUse = originalImage;
        }
        
        //compress image
        imageToUse = [self compressImage:imageToUse];
        
        // Do something with imageToUse
        NSData *imageData = UIImagePNGRepresentation(imageToUse);
        NSArray *paths = NSSearchPathForDirectoriesInDomains(NSDocumentDirectory, NSUserDomainMask, YES);
        NSString *documentsDirectory = [paths objectAtIndex:0];
        NSString *filePath = [documentsDirectory stringByAppendingPathComponent:@"selected.jpg"]; //Add the file name
        NSLog(@"filePath %@",filePath);
        [imageData writeToFile:filePath atomically:YES];
        
        //Attempt to forcibly eject memory hogs
        originalImage = nil;
        editedImage = nil;
        imageToUse = nil;
        imageData = nil;
        
        UnitySendMessage("LevelManager","ReceivePhotoPath",[filePath UTF8String]);
        filePath = nil;
        //return MakeStringCopy([filePath UTF8String]);
    }
    //}
    
    
    
    [picker dismissModalViewControllerAnimated: YES];
    // [picker release];
}

- (void)imagePickerControllerDidCancel:(UIImagePickerController *)picker
{
    //cancelled
    [picker dismissModalViewControllerAnimated: YES];
}



- (UIImage *)compressImage:(UIImage *)image {
    float actualHeight = image.size.height;
    float actualWidth = image.size.width;
    float maxHeight = 800.0; //new max. height for image
    float maxWidth = 600.0; //new max. width for image
    float imgRatio = actualWidth/actualHeight;
    float maxRatio = maxWidth/maxHeight;
    float compressionQuality = 0.85; //15 percent compression
    
    if (actualHeight > maxHeight || actualWidth > maxWidth){
        if(imgRatio < maxRatio){
            //adjust width according to maxHeight
            imgRatio = maxHeight / actualHeight;
            actualWidth = imgRatio * actualWidth;
            actualHeight = maxHeight;
        }
        else if(imgRatio > maxRatio){
            //adjust height according to maxWidth
            imgRatio = maxWidth / actualWidth;
            actualHeight = imgRatio * actualHeight;
            actualWidth = maxWidth;
        }
        else{
            actualHeight = maxHeight;
            actualWidth = maxWidth;
        }
    }
    NSLog(@"Actual height : %f and Width : %f",actualHeight,actualWidth);
    CGRect rect = CGRectMake(0.0, 0.0, actualWidth, actualHeight);
    UIGraphicsBeginImageContext(rect.size);
    [image drawInRect:rect];
    UIImage *img = UIGraphicsGetImageFromCurrentImageContext();
    NSData *imageData = UIImageJPEGRepresentation(img, compressionQuality);
    UIGraphicsEndImageContext();
    
    return [UIImage imageWithData:imageData];
}


- (void)ShareToFacebook:(const char*)msg
{
    NSString *nsmsg = [NSString stringWithUTF8String:msg];
    
    if([SLComposeViewController isAvailableForServiceType:SLServiceTypeFacebook])
    {
        
        NSUserDefaults *defaults = [NSUserDefaults standardUserDefaults];
        NSString *path = [defaults objectForKey:@"shotToSave"];
        UIImage *imageToShare = [UIImage imageWithContentsOfFile:path];
        
        SLComposeViewController  *mySLComposerSheet = [[SLComposeViewController alloc] init];
        mySLComposerSheet = [SLComposeViewController composeViewControllerForServiceType:SLServiceTypeFacebook];
        [mySLComposerSheet setInitialText:nsmsg];
        [mySLComposerSheet addImage:imageToShare];
        
        [mySLComposerSheet setCompletionHandler:^(SLComposeViewControllerResult result)
         {
             
             switch (result) {
                 case SLComposeViewControllerResultCancelled:
                     NSLog(@"Post Cancelled");
                     break;
                 case SLComposeViewControllerResultDone:
                 {
                     NSLog(@"Post Shared");
                     UIAlertView *alertView = [[UIAlertView alloc]initWithTitle:
                                               @"Thanks For Sharing!" message:nil delegate:
                                               self cancelButtonTitle:@"Ok" otherButtonTitles: nil];
                     [alertView show];
                     
                     //UnitySendMessage("NerdPrimePlugin","_SocialShareCallback",[@"" UTF8String]);
                 }
                     
                 default:
                     break;
             }
         }];
        
        
        [UnityGetGLViewController() presentViewController:mySLComposerSheet animated:YES completion:nil];
        
        
    }
    
    else
    {
        
        UIAlertView *alertView = [[UIAlertView alloc]initWithTitle:
                                  @"Facebook is unavailable at this time" message:nil delegate:
                                  self cancelButtonTitle:@"Ok" otherButtonTitles: nil];
        [alertView show];
    }
    
    
}

- (void)ShareToFacebookCard:(const char*)msg
{
    
    NSString *nsmsg = [NSString stringWithUTF8String:msg];
    if([SLComposeViewController isAvailableForServiceType:SLServiceTypeFacebook])
    {
        NSUserDefaults *defaults = [NSUserDefaults standardUserDefaults];
        NSString *path = [defaults objectForKey:@"promoImage"];
        UIImage *imageToShare = [UIImage imageWithContentsOfFile:path];
        
        SLComposeViewController  *mySLComposerSheet = [[SLComposeViewController alloc] init];
        mySLComposerSheet = [SLComposeViewController composeViewControllerForServiceType:SLServiceTypeFacebook];
        [mySLComposerSheet setInitialText:nsmsg];
        [mySLComposerSheet addImage:imageToShare];
        
        [mySLComposerSheet setCompletionHandler:^(SLComposeViewControllerResult result)
         {
             
             switch (result) {
                 case SLComposeViewControllerResultCancelled:
                     NSLog(@"Post Cancelled");
                     break;
                 case SLComposeViewControllerResultDone:
                 {
                     NSLog(@"Post Shared");
                     UIAlertView *alertView = [[UIAlertView alloc]initWithTitle:
                                               @"Thanks For Sharing!" message:nil delegate:
                                               self cancelButtonTitle:@"Ok" otherButtonTitles: nil];
                     [alertView show];
                     
                     UnitySendMessage("NerdPrimePlugin","_SocialShareCallback",[@"" UTF8String]);
                 }
                     
                 default:
                     break;
             }
         }];
        
        
        [UnityGetGLViewController() presentViewController:mySLComposerSheet animated:YES completion:nil];
        NSLog(@"Shared");
    }
    
    else
    {
        
        UIAlertView *alertView = [[UIAlertView alloc]initWithTitle:
                                  @"Facebook is unavailable at this time" message:nil delegate:
                                  self cancelButtonTitle:@"Ok" otherButtonTitles: nil];
        [alertView show];
    }
    
    
}

- (void)ShareToTwitter:(const char*)msg
{
    
    NSString *nsmsg = [NSString stringWithUTF8String:msg];
    if([SLComposeViewController isAvailableForServiceType:SLServiceTypeTwitter])
    {
        
        NSUserDefaults *defaults = [NSUserDefaults standardUserDefaults];
        NSString *path = [defaults objectForKey:@"shotToSave"];
        UIImage *imageToShare = [UIImage imageWithContentsOfFile:path];
        
        SLComposeViewController  *mySLComposerSheet = [[SLComposeViewController alloc] init];
        mySLComposerSheet = [SLComposeViewController composeViewControllerForServiceType:SLServiceTypeTwitter];
        [mySLComposerSheet setInitialText:nsmsg];
        [mySLComposerSheet addImage:imageToShare];
        
        [mySLComposerSheet setCompletionHandler:^(SLComposeViewControllerResult result)
         {
             
             switch (result) {
                 case SLComposeViewControllerResultCancelled:
                     NSLog(@"Post Cancelled");
                     break;
                 case SLComposeViewControllerResultDone:
                 {
                     NSLog(@"Post Shared");
                     UIAlertView *alertView = [[UIAlertView alloc]initWithTitle:
                                               @"Thanks For Sharing!" message:nil delegate:
                                               self cancelButtonTitle:@"Ok" otherButtonTitles: nil];
                     [alertView show];
                     
                    // UnitySendMessage("NerdPrimePlugin","_SocialShareCallback",[@"" UTF8String]);
                 }
                     
                 default:
                     break;
             }
         }];
        
        
        [UnityGetGLViewController() presentViewController:mySLComposerSheet animated:YES completion:nil];
        NSLog(@"Shared");
    }
    
    else
    {
        
        UIAlertView *alertView = [[UIAlertView alloc]initWithTitle:
                                  @"Twitter is unavailable at this time" message:nil delegate:
                                  self cancelButtonTitle:@"Ok" otherButtonTitles: nil];
        [alertView show];
    }
    
    
}

- (void)ShareToTwitterCard:(const char*)msg
{
    
    NSString *nsmsg = [NSString stringWithUTF8String:msg];
    if([SLComposeViewController isAvailableForServiceType:SLServiceTypeTwitter])
    {
        
        NSUserDefaults *defaults = [NSUserDefaults standardUserDefaults];
        NSString *path = [defaults objectForKey:@"promoImage"];
        UIImage *imageToShare = [UIImage imageWithContentsOfFile:path];
        
        SLComposeViewController  *mySLComposerSheet = [[SLComposeViewController alloc] init];
        mySLComposerSheet = [SLComposeViewController composeViewControllerForServiceType:SLServiceTypeTwitter];
        [mySLComposerSheet setInitialText:nsmsg];
        [mySLComposerSheet addImage:imageToShare];
        
        [mySLComposerSheet setCompletionHandler:^(SLComposeViewControllerResult result)
         {
             
             switch (result) {
                 case SLComposeViewControllerResultCancelled:
                     NSLog(@"Post Cancelled");
                     break;
                 case SLComposeViewControllerResultDone:
                 {
                     NSLog(@"Post Shared");
                     UIAlertView *alertView = [[UIAlertView alloc]initWithTitle:
                                               @"Thanks For Sharing!" message:nil delegate:
                                               self cancelButtonTitle:@"Ok" otherButtonTitles: nil];
                     [alertView show];
                     
                     UnitySendMessage("NerdPrimePlugin","_SocialShareCallback",[@"" UTF8String]);
                 }
                     
                 default:
                     break;
             }
         }];
        
        
        [UnityGetGLViewController() presentViewController:mySLComposerSheet animated:YES completion:nil];
        NSLog(@"Shared");
    }
    
    else
    {
        
        UIAlertView *alertView = [[UIAlertView alloc]initWithTitle:
                                  @"Twitter is unavailable at this time" message:nil delegate:
                                  self cancelButtonTitle:@"Ok" otherButtonTitles: nil];
        [alertView show];
    }
    
    
}

-(void)ShowAlert:(const char*)msg
{
    NSString *nsmsg = [NSString stringWithUTF8String:msg];
    UIAlertView *alertView = [[UIAlertView alloc]initWithTitle:nsmsg message:nil delegate:
                              self cancelButtonTitle:@"Ok" otherButtonTitles: nil];
    [alertView show];
}

-(void)RateUs:(const char*)appid
{
    //    SKStoreProductViewController *storeView = [[SKStoreProductViewController alloc] init];
    //    [storeView setDelegate:self];
    //
    //    NSDictionary *params = @{ SKStoreProductParameterITunesItemIdentifier : @"1012552008" };
    //    [storeView loadProductWithParameters:params completionBlock:^(BOOL result, NSError *error)
    //     {
    //         if(result == YES)
    //         {
    //             [UnityGetGLViewController() presentViewController:storeView animated: YES completion:nil];
    //             NSLog(@"Rated app");
    //         }
    //         else
    //         {
    //             NSLog(@"Rate mee eror");
    //         }
    //     }];
    
    NSString *nsmsg = [NSString stringWithUTF8String:appid];
    
    NSString *path = [NSString stringWithFormat:@"itms-apps://itunes.apple.com/app/%@",nsmsg];
    [[UIApplication sharedApplication] openURL:[NSURL URLWithString:path]];
}

-(void)MoreGames
{
    [[UIApplication sharedApplication] openURL:[NSURL URLWithString:@"https://itunes.apple.com/gb/artist/nerd-agency/id933203643"]];
}

-(void)DownloadGame:(const char*)_gameId
{
    NSString *nsmsg = [NSString stringWithUTF8String:_gameId];
    NSString *gameUrl = [NSString stringWithFormat: @"%@%@",@"itms-apps://itunes.apple.com/app/",nsmsg];
    [[UIApplication sharedApplication] openURL:[NSURL URLWithString: gameUrl]];
}


-(void)VisitWebpage:(const char*)_address
{
    NSString *site;
    NSString *nsmsg = [NSString stringWithUTF8String:_address];
    
    if ([nsmsg isEqualToString:@"facebook"])
    {
        site = @"fb://profile/nerdagencygames";
        if(![[UIApplication sharedApplication] openURL:[NSURL URLWithString: site]])
        {
            site = @"https://www.facebook.com/nerdagencygames";
        }
    }
    else if([nsmsg isEqualToString:@"twitter"])
    {
        site = @"https://www.twitter.com/nerdagencygames";
    }
    else if([nsmsg isEqualToString:@"instagram"])
    {
        site = @"https://www.instagram.com/nerdagencygames";
    }
    
    //NSString *url = [NSString stringWithFormat: @"%@%@",@"itms-apps://itunes.apple.com/app/",nsmsg];
    [[UIApplication sharedApplication] openURL:[NSURL URLWithString: site]];
}


-(void)productViewControllerDidFinish:(SKStoreProductViewController *)viewController
{
    //[viewController dismissModalViewControllerAnimated:YES];
    [viewController dismissViewControllerAnimated:YES completion:nil];
}


@end