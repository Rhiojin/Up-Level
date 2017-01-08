//
//  SaveSS.h
//  SaveToAlbumTest
//
//  Created by Rheo Violenes on 11/02/15.
//  Copyright (c) 2015 f4f. All rights reserved.
//

#ifndef SaveToAlbumTest_SaveSS_h
#define SaveToAlbumTest_SaveSS_h


#endif

#import <Foundation/Foundation.h>
@import StoreKit;


void _SaveToAlbum();
void _ChoosePhoto();
void _ShareToFacebook(const char* msg);
void _ShareToTwitter(const char* msg);

void _ShareToFacebookCard(const char* msg);
void _ShareToTwitterCard(const char* msg);

void _ShowAlert(const char* msg);
void _RateUs(const char* _id);
void _MoreGames();
void _DownloadGame(const char* gameId);
void _VisitWebpage(const char* address);



@interface SaveSS : NSObject

- (BOOL)startMediaBrowserFromViewController: (UIViewController*) controller
                              usingDelegate: (id <UIImagePickerControllerDelegate,
                                              UINavigationControllerDelegate>) delegate;

- (void)imagePickerController:(UIImagePickerController *)picker
didFinishPickingMediaWithInfo:(NSDictionary *)info;

- (void)imagePickerControllerDidCancel:(UIImagePickerController *)picker;
- (void)productViewControllerDidFinish:(SKStoreProductViewController *)viewController;



// public methods
- (void)SaveToAlbum;
- (void)ChoosePhoto;
- (void)ShareToFacebook:(const char*)msg;
- (void)ShareToTwitter:(const char*)msg;

- (void)ShareToFacebookCard:(const char*)msg;
- (void)ShareToTwitterCard:(const char*)msg;

- (void)ShowAlert:(const char*)msg;
- (void)RateUs:(const char*)_appId;
- (void)MoreGames;
- (void)DownloadGame:(const char*)_gameId;
- (void)VisitWebpage:(const char*)_address;

- (UIImage *)compressImage:(UIImage *)image;



@end
